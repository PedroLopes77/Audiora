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
import com.audiora.app.models.Playlist;
import com.bumptech.glide.Glide;
import java.util.List;

public class PlaylistAdapter extends RecyclerView.Adapter<PlaylistAdapter.ViewHolder> {

    public interface OnPlaylistClickListener {
        void onPlaylistClick(Playlist playlist);
    }

    private final Context context;
    private List<Playlist> playlists;
    private OnPlaylistClickListener listener;

    public PlaylistAdapter(Context context, List<Playlist> playlists) {
        this.context   = context;
        this.playlists = playlists;
    }

    public void setListener(OnPlaylistClickListener listener) { this.listener = listener; }

    public void updatePlaylists(List<Playlist> newPlaylists) {
        this.playlists = newPlaylists;
        notifyDataSetChanged();
    }

    @NonNull @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(context).inflate(R.layout.item_playlist, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        Playlist playlist = playlists.get(position);
        holder.tvName.setText(playlist.getName());
        holder.tvTracks.setText(playlist.getTotalTracks() + " músicas");

        if (playlist.getCoverImageUrl() != null && !playlist.getCoverImageUrl().isEmpty()) {
            Glide.with(context)
                    .load(playlist.getCoverImageUrl())
                    .placeholder(R.drawable.ic_music_placeholder)
                    .into(holder.ivCover);
        } else {
            holder.ivCover.setImageResource(R.drawable.ic_music_placeholder);
        }

        holder.itemView.setOnClickListener(v -> { if (listener != null) listener.onPlaylistClick(playlist); });
    }

    @Override public int getItemCount() { return playlists != null ? playlists.size() : 0; }

    static class ViewHolder extends RecyclerView.ViewHolder {
        ImageView ivCover;
        TextView tvName, tvTracks;

        ViewHolder(View view) {
            super(view);
            ivCover  = view.findViewById(R.id.iv_cover);
            tvName   = view.findViewById(R.id.tv_name);
            tvTracks = view.findViewById(R.id.tv_tracks);
        }
    }
}