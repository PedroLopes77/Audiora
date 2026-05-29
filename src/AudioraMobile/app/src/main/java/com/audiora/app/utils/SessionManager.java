package com.audiora.app.utils;

import android.content.Context;
import android.content.SharedPreferences;

public class SessionManager {
    private final SharedPreferences prefs;
    private final SharedPreferences.Editor editor;

    public SessionManager(Context context) {
        prefs  = context.getSharedPreferences(Constants.PREF_NAME, Context.MODE_PRIVATE);
        editor = prefs.edit();
    }

    public void saveToken(String token) {
        editor.putString(Constants.KEY_TOKEN, token);
        editor.apply();
    }

    public String getToken() {
        return prefs.getString(Constants.KEY_TOKEN, null);
    }

    public void saveUser(String name, String email, boolean isPremium) {
        editor.putString(Constants.KEY_USER_NAME, name);
        editor.putString(Constants.KEY_USER_EMAIL, email);
        editor.putBoolean(Constants.KEY_IS_PREMIUM, isPremium);
        editor.apply();
    }

    public String getUserName()  { return prefs.getString(Constants.KEY_USER_NAME, ""); }
    public String getUserEmail() { return prefs.getString(Constants.KEY_USER_EMAIL, ""); }
    public boolean isPremium()   { return prefs.getBoolean(Constants.KEY_IS_PREMIUM, false); }

    public boolean isLoggedIn() { return getToken() != null; }

    public void logout() {
        editor.clear();
        editor.apply();
    }
}