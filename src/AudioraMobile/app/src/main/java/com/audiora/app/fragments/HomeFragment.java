package com.audiora.app.fragments;

import android.os.Bundle;
import android.view.*;
import android.widget.Toast;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import com.audiora.app.R;
import com.audiora.app.adapters.MusicAdapter;
import com.audiora.app.api.ApiClient;
import com.audiora.app.api.ApiService;
import com.audiora.app.databinding.FragmentHomeBinding;
import com.audiora.app.models.*;
import com.audiora.app.player.AudioPlayerManager;
import com.audiora.app.utils.SessionManager;
import java.util.ArrayList;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class HomeFragment extends Fragment {

    private FragmentHomeBinding binding;
    private MusicAdapter adapter;
    private ApiService apiService;
    private SessionManager sessionManager;

    @Nullable @Override
    public View onCreateView(@NonNull LayoutInflater inflater,
                             @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        binding = FragmentHomeBinding.inflate(inflater, container, false);
        return binding.getRoot();
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        apiService     = ApiClient.getService(requireContext());
        sessionManager = new SessionManager(requireContext());

        adapter = new MusicAdapter(requireContext(), new ArrayList<>());
        adapter.setListener(new MusicAdapter.OnMusicClickListener() {
            @Override public void onMusicClick(Music music) { playMusic(music); }
            @Override public void onMoreClick(Music music)  { /* menu futuro */ }
        });

        binding.rvMusics.setLayoutManager(new LinearLayoutManager(requireContext()));
        binding.rvMusics.setAdapter(adapter);

        loadMusics();
    }

    private void loadMusics() {
        binding.progressBar.setVisibility(android.view.View.VISIBLE);

        apiService.getMusics(1, 20).enqueue(new Callback<ApiResponse<PagedResponse<Music>>>() {
            @Override
            public void onResponse(Call<ApiResponse<PagedResponse<Music>>> call,
                                   Response<ApiResponse<PagedResponse<Music>>> response) {
                binding.progressBar.setVisibility(android.view.View.GONE);
                if (response.isSuccessful() && response.body() != null && response.body().isSuccess()) {
                    PagedResponse<Music> paged = response.body().getData();
                    if (paged != null && paged.getData() != null) {
                        adapter.updateMusics(paged.getData());
                    }
                }
            }

            @Override
            public void onFailure(Call<ApiResponse<PagedResponse<Music>>> call, Throwable t) {
                binding.progressBar.setVisibility(android.view.View.GONE);
                Toast.makeText(requireContext(), "Falha ao carregar músicas", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void playMusic(Music music) {
        String token = sessionManager.getToken();
        AudioPlayerManager.getInstance().play(requireContext(), music, token);
        Toast.makeText(requireContext(), "▶ " + music.getTitle(), Toast.LENGTH_SHORT).show();

        // Registra stream na API
        apiService.registerStream(music.getId()).enqueue(new Callback<ApiResponse<Boolean>>() {
            @Override public void onResponse(Call<ApiResponse<Boolean>> c, Response<ApiResponse<Boolean>> r) {}
            @Override public void onFailure(Call<ApiResponse<Boolean>> c, Throwable t) {}
        });
    }

    @Override public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}