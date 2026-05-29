package com.audiora.app.fragments;

import android.os.Bundle;
import android.view.*;
import android.widget.Toast;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import com.audiora.app.adapters.PlaylistAdapter;
import com.audiora.app.api.ApiClient;
import com.audiora.app.api.ApiService;
import com.audiora.app.databinding.FragmentLibraryBinding;
import com.audiora.app.models.*;
import java.util.ArrayList;
import java.util.List;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LibraryFragment extends Fragment {

    private FragmentLibraryBinding binding;
    private PlaylistAdapter adapter;
    private ApiService apiService;

    @Nullable @Override
    public View onCreateView(@NonNull LayoutInflater inflater,
                             @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        binding = FragmentLibraryBinding.inflate(inflater, container, false);
        return binding.getRoot();
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        apiService = ApiClient.getService(requireContext());

        adapter = new PlaylistAdapter(requireContext(), new ArrayList<>());
        adapter.setListener(playlist ->
                Toast.makeText(requireContext(), "Playlist: " + playlist.getName(), Toast.LENGTH_SHORT).show());

        binding.rvPlaylists.setLayoutManager(new LinearLayoutManager(requireContext()));
        binding.rvPlaylists.setAdapter(adapter);

        loadPlaylists();
    }

    private void loadPlaylists() {
        binding.progressBar.setVisibility(android.view.View.VISIBLE);

        apiService.getMyPlaylists().enqueue(new Callback<ApiResponse<List<Playlist>>>() {
            @Override
            public void onResponse(Call<ApiResponse<List<Playlist>>> call,
                                   Response<ApiResponse<List<Playlist>>> response) {
                binding.progressBar.setVisibility(android.view.View.GONE);
                if (response.isSuccessful() && response.body() != null && response.body().isSuccess()) {
                    List<Playlist> playlists = response.body().getData();
                    if (playlists != null) adapter.updatePlaylists(playlists);
                }
            }
            @Override public void onFailure(Call<ApiResponse<List<Playlist>>> call, Throwable t) {
                binding.progressBar.setVisibility(android.view.View.GONE);
            }
        });
    }

    @Override public void onDestroyView() { super.onDestroyView(); binding = null; }
}