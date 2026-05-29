package com.audiora.app.models;

public class RegisterRequest {
    private String name;
    private String email;
    private String password;
    private String confirmPassword;
    private String birthDate;
    private String country;

    public RegisterRequest(String name, String email, String password,
                           String confirmPassword, String birthDate, String country) {
        this.name            = name;
        this.email           = email;
        this.password        = password;
        this.confirmPassword = confirmPassword;
        this.birthDate       = birthDate;
        this.country         = country;
    }
}