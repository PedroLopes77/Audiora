package com.audiora.app.adapters;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.audiora.app.R;
import com.audiora.app.models.Music;
import com.bumptech.glide.Glide;
import java.util.List;

public class MusicAdapter extends RecyclerView.Adapter<MusicAdapter.ViewHolder> {

    public interface OnMusicClickListener {
        void onMusicClick(Music music);
        void onMoreClick(Music music);
    }

    private final Context context;
    private List<Music> musics;
    private OnMusicClickListener listener;

    public MusicAdapter(Context context, List<Music> musics) {
        this.context = context;
        this.musics  = musics;
    }

    public void setListener(OnMusicClickListener listener) { this.listener = listener; }

    public void updateMusics(List<Music> newMusics) {
        this.musics = newMusics;
        notifyDataSetChanged();
    }

    @NonNull @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(context).inflate(R.layout.item_music, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        Music music = musics.get(position);

        holder.tvTitle.setText(music.getTitle());
        holder.tvArtist.setText(music.getArtistName() != null ? music.getArtistName() : "Artista desconhecido");
        holder.tvDuration.setText(music.getDuration() != null ? music.getDuration() : "");

        if (music.getCoverImageUrl() != null && !music.getCoverImageUrl().isEmpty()) {
            Glide.with(context)
                    .load(music.getCoverImageUrl())
                    .placeholder(R.drawable.ic_music_placeholder)
                    .error(R.drawable.ic_music_placeholder)
                    .into(holder.ivCover);
        } else {
            holder.ivCover.setImageResource(R.drawable.ic_music_placeholder);
        }

        holder.itemView.setOnClickListener(v -> { if (listener != null) listener.onMusicClick(music); });
        holder.ivMore.setOnClickListener(v  -> { if (listener != null) listener.onMoreClick(music); });
    }

    @Override public int getItemCount() { return musics != null ? musics.size() : 0; }

    static class ViewHolder extends RecyclerView.ViewHolder {
        ImageView ivCover, ivMore;
        TextView tvTitle, tvArtist, tvDuration;

        ViewHolder(View view) {
            super(view);
            ivCover    = view.findViewById(R.id.iv_cover);
            ivMore     = view.findViewById(R.id.iv_more);
            tvTitle    = view.findViewById(R.id.tv_title);
            tvArtist   = view.findViewById(R.id.tv_artist);
            tvDuration = view.findViewById(R.id.tv_duration);
        }
    }
}