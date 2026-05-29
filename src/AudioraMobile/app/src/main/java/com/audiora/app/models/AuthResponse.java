package com.audiora.app.models;

public class AuthResponse {
    private String token;
    private String name;
    private String email;
    private String role;
    private boolean isPremium;

    public String getToken()   { return token; }
    public String getName()    { return name; }
    public String getEmail()   { return email; }
    public String getRole()    { return role; }
    public boolean isPremium() { return isPremium; }
}