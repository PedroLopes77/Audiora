package com.audiora.app.player;

import android.content.Context;
import android.media.AudioAttributes;
import android.media.MediaPlayer;
import android.net.Uri;
import android.os.Handler;
import android.os.Looper;
import com.audiora.app.models.Music;
import java.util.HashMap;
import java.util.Map;

public class AudioPlayerManager {

    public interface PlayerListener {
        void onPrepared(Music music);
        void onProgress(int current, int total);
        void onCompleted();
        void onError(String message);
    }

    private static AudioPlayerManager instance;
    private MediaPlayer mediaPlayer;
    private Music currentMusic;
    private boolean isPrepared = false;
    private PlayerListener listener;
    private final Handler handler = new Handler(Looper.getMainLooper());
    private Runnable progressRunnable;

    public static AudioPlayerManager getInstance() {
        if (instance == null) instance = new AudioPlayerManager();
        return instance;
    }

    public void setListener(PlayerListener listener) {
        this.listener = listener;
    }

    public void play(Context context, Music music, String token) {
        if (currentMusic != null && currentMusic.getId().equals(music.getId()) && isPrepared) {
            if (!mediaPlayer.isPlaying()) {
                mediaPlayer.start();
                startProgressUpdater();
            }
            return;
        }

        stop();
        currentMusic = music;
        isPrepared = false;

        mediaPlayer = new MediaPlayer();
        mediaPlayer.setAudioAttributes(new AudioAttributes.Builder()
                .setContentType(AudioAttributes.CONTENT_TYPE_MUSIC)
                .setUsage(AudioAttributes.USAGE_MEDIA)
                .build());

        try {
            Map<String, String> headers = new HashMap<>();
            if (token != null) headers.put("Authorization", "Bearer " + token);

            mediaPlayer.setDataSource(context, Uri.parse(music.getAudioUrl()), headers);
            mediaPlayer.prepareAsync();

            mediaPlayer.setOnPreparedListener(mp -> {
                isPrepared = true;
                mp.start();
                if (listener != null) listener.onPrepared(music);
                startProgressUpdater();
            });

            mediaPlayer.setOnCompletionListener(mp -> {
                stopProgressUpdater();
                if (listener != null) listener.onCompleted();
            });

            mediaPlayer.setOnErrorListener((mp, what, extra) -> {
                if (listener != null) listener.onError("Erro ao reproduzir áudio (código: " + what + ")");
                return true;
            });

        } catch (Exception e) {
            if (listener != null) listener.onError(e.getMessage());
        }
    }

    public void pause() {
        if (mediaPlayer != null && mediaPlayer.isPlaying()) {
            mediaPlayer.pause();
            stopProgressUpdater();
        }
    }

    public void resume() {
        if (mediaPlayer != null && isPrepared && !mediaPlayer.isPlaying()) {
            mediaPlayer.start();
            startProgressUpdater();
        }
    }

    public void stop() {
        stopProgressUpdater();
        if (mediaPlayer != null) {
            try { mediaPlayer.stop(); } catch (Exception ignored) {}
            mediaPlayer.release();
            mediaPlayer = null;
        }
        isPrepared = false;
        currentMusic = null;
    }

    public void seekTo(int positionMs) {
        if (mediaPlayer != null && isPrepared) mediaPlayer.seekTo(positionMs);
    }

    public boolean isPlaying()      { return mediaPlayer != null && mediaPlayer.isPlaying(); }
    public Music getCurrentMusic()  { return currentMusic; }

    public int getCurrentPosition() {
        return (mediaPlayer != null && isPrepared) ? mediaPlayer.getCurrentPosition() : 0;
    }

    public int getDuration() {
        return (mediaPlayer != null && isPrepared) ? mediaPlayer.getDuration() : 0;
    }

    private void startProgressUpdater() {
        progressRunnable = new Runnable() {
            @Override public void run() {
                if (mediaPlayer != null && mediaPlayer.isPlaying() && listener != null) {
                    listener.onProgress(mediaPlayer.getCurrentPosition(), mediaPlayer.getDuration());
                    handler.postDelayed(this, 1000);
                }
            }
        };
        handler.post(progressRunnable);
    }

    private void stopProgressUpdater() {
        if (progressRunnable != null) handler.removeCallbacks(progressRunnable);
    }
}