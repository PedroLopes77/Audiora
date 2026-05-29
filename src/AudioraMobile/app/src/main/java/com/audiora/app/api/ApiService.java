package com.audiora.app.api;

import com.audiora.app.models.*;
import retrofit2.Call;
import retrofit2.http.*;
import java.util.List;

public interface ApiService {

    // ── Auth ──────────────────────────────────────────────────────────────
    @POST("api/Auth/login")
    Call<ApiResponse<AuthResponse>> login(@Body LoginRequest request);

    @POST("api/Auth/register")
    Call<ApiResponse<AuthResponse>> register(@Body RegisterRequest request);

    @GET("api/Auth/profile")
    Call<ApiResponse<Object>> getProfile();

    // ── Music ─────────────────────────────────────────────────────────────
    @GET("api/Music")
    Call<ApiResponse<PagedResponse<Music>>> getMusics(
            @Query("page") int page,
            @Query("pageSize") int pageSize
    );

    @GET("api/Music/search")
    Call<ApiResponse<PagedResponse<Music>>> searchMusics(
            @Query("term") String term,
            @Query("page") int page,
            @Query("pageSize") int pageSize
    );

    @POST("api/Music/{id}/stream")
    Call<ApiResponse<Boolean>> registerStream(@Path("id") String id);

    // ── Playlist ──────────────────────────────────────────────────────────
    @GET("api/Playlist/my")
    Call<ApiResponse<List<Playlist>>> getMyPlaylists();

    @GET("api/Playlist/{id}")
    Call<ApiResponse<Playlist>> getPlaylist(@Path("id") String id);

    // ── Favorites ─────────────────────────────────────────────────────────
    @GET("api/Favorite")
    Call<ApiResponse<List<Object>>> getFavorites();

    @POST("api/Favorite/{musicId}/toggle")
    Call<ApiResponse<Boolean>> toggleFavorite(@Path("musicId") String musicId);

    // ── History / Recommendations ─────────────────────────────────────────
    @GET("api/History/recommendations")
    Call<ApiResponse<List<Music>>> getRecommendations();
}