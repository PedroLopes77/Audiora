package com.audiora.app.activities;

import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;
import com.audiora.app.R;
import com.audiora.app.databinding.ActivityMainBinding;
import com.audiora.app.fragments.HomeFragment;
import com.audiora.app.fragments.LibraryFragment;
import com.audiora.app.fragments.ProfileFragment;
import com.audiora.app.fragments.SearchFragment;

public class MainActivity extends AppCompatActivity {

    private ActivityMainBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = ActivityMainBinding.inflate(getLayoutInflater());
        setContentView(binding.getRoot());

        loadFragment(new HomeFragment());

        binding.bottomNav.setOnItemSelectedListener(item -> {
            int id = item.getItemId();
            Fragment fragment = null;

            if      (id == R.id.nav_home)    fragment = new HomeFragment();
            else if (id == R.id.nav_search)  fragment = new SearchFragment();
            else if (id == R.id.nav_library) fragment = new LibraryFragment();
            else if (id == R.id.nav_profile) fragment = new ProfileFragment();

            if (fragment != null) { loadFragment(fragment); return true; }
            return false;
        });
    }

    private void loadFragment(Fragment fragment) {
        getSupportFragmentManager().beginTransaction()
                .replace(R.id.fragment_container, fragment)
                .commit();
    }
}