using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Audiora.Admin.Helpers;
using Audiora.Admin.Models;

namespace Audiora.Admin.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private const string BaseUrl = "http://localhost:5003/";

    public ApiService()
    {
        _http = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    private void SetAuthHeader()
    {
        var token = SessionHelper.GetToken();
        if (!string.IsNullOrEmpty(token))
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    // ── Auth ──────────────────────────────────────────────────────────────
    public async Task<ApiResponse<AuthResponse>?> LoginAsync(string email, string password)
    {
        var body = JsonSerializer.Serialize(new { email, password });
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("api/Auth/login", content);
        return await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
    }

    // ── Music ─────────────────────────────────────────────────────────────
    public async Task<ApiResponse<PagedResponse<MusicModel>>?> GetMusicsAsync(int page = 1, int pageSize = 20)
    {
        SetAuthHeader();
        return await _http.GetFromJsonAsync<ApiResponse<PagedResponse<MusicModel>>>(
            $"api/Music?page={page}&pageSize={pageSize}");
    }

    public async Task<bool> DeleteMusicAsync(string id)
    {
        SetAuthHeader();
        var response = await _http.DeleteAsync($"api/Music/{id}");
        return response.IsSuccessStatusCode;
    }
}