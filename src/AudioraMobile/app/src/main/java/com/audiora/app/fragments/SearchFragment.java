package com.audiora.app.fragments;

import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.*;
import android.widget.Toast;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import com.audiora.app.adapters.MusicAdapter;
import com.audiora.app.api.ApiClient;
import com.audiora.app.api.ApiService;
import com.audiora.app.databinding.FragmentSearchBinding;
import com.audiora.app.models.*;
import com.audiora.app.player.AudioPlayerManager;
import com.audiora.app.utils.SessionManager;
import java.util.ArrayList;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class SearchFragment extends Fragment {

    private FragmentSearchBinding binding;
    private MusicAdapter adapter;
    private ApiService apiService;
    private SessionManager sessionManager;

    @Nullable @Override
    public View onCreateView(@NonNull LayoutInflater inflater,
                             @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        binding = FragmentSearchBinding.inflate(inflater, container, false);
        return binding.getRoot();
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        apiService     = ApiClient.getService(requireContext());
        sessionManager = new SessionManager(requireContext());

        adapter = new MusicAdapter(requireContext(), new ArrayList<>());
        adapter.setListener(new MusicAdapter.OnMusicClickListener() {
            @Override public void onMusicClick(Music music) {
                AudioPlayerManager.getInstance().play(requireContext(), music, sessionManager.getToken());
                Toast.makeText(requireContext(), "▶ " + music.getTitle(), Toast.LENGTH_SHORT).show();
            }
            @Override public void onMoreClick(Music music) {}
        });

        binding.rvResults.setLayoutManager(new LinearLayoutManager(requireContext()));
        binding.rvResults.setAdapter(adapter);

        binding.etSearch.addTextChangedListener(new TextWatcher() {
            @Override public void beforeTextChanged(CharSequence s, int start, int count, int after) {}
            @Override public void onTextChanged(CharSequence s, int start, int before, int count) {
                String term = s.toString().trim();
                if (term.length() >= 2) search(term);
                else if (term.isEmpty()) adapter.updateMusics(new ArrayList<>());
            }
            @Override public void afterTextChanged(Editable s) {}
        });
    }

    private void search(String term) {
        apiService.searchMusics(term, 1, 20).enqueue(new Callback<ApiResponse<PagedResponse<Music>>>() {
            @Override
            public void onResponse(Call<ApiResponse<PagedResponse<Music>>> call,
                                   Response<ApiResponse<PagedResponse<Music>>> response) {
                if (response.isSuccessful() && response.body() != null && response.body().isSuccess()) {
                    PagedResponse<Music> paged = response.body().getData();
                    if (paged != null && paged.getData() != null) adapter.updateMusics(paged.getData());
                }
            }
            @Override public void onFailure(Call<ApiResponse<PagedResponse<Music>>> call, Throwable t) {}
        });
    }

    @Override public void onDestroyView() { super.onDestroyView(); binding = null; }
}