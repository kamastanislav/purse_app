package com.purse.purseclient;

import android.app.ProgressDialog;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.view.View;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Toast;

import com.purse.entity.UserData;
import com.purse.helper.Constants;
import com.purse.services.RestService;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MenuActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    private ProgressDialog progress;
    private Intent intent;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_menu);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        progress = new ProgressDialog(this);
        intent = new Intent(this, LoginActivity.class);

        loadStartFragment();

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.setDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);

        loadUserName();
    }

    private void loadUserName() {
        Call<UserData> call = RestService.getService().getSessionUser();
        call.enqueue(new Callback<UserData>() {
            @Override
            public void onResponse(Call<UserData> call, Response<UserData> response) {
                if (response.isSuccessful()) {
                    UserData userData = response.body();
                    Constants.userName = userData != null ? String.format("%1$s %2$s (%3$s)", userData.FirstName, userData.LastName, userData.NickName) : "";
                    Constants.userCode = userData != null ? userData.Code : Constants.DEFAULT_CODE;
                    Constants.familyCode = userData != null ? userData.FamilyCode : Constants.DEFAULT_CODE;
                }
            }

            @Override
            public void onFailure(Call<UserData> call, Throwable t) {

            }
        });
    }

    private void loadStartFragment() {
        Fragment fragment = new HistoryFragment();
        FragmentTransaction ft = getSupportFragmentManager().beginTransaction();
        ft.replace(R.id.content_frame, fragment);
        ft.commit();

        if (getSupportActionBar() != null) {
            getSupportActionBar().setTitle("История");
        }
    }

    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        int id = item.getItemId();

        Fragment fragment = null;
        String title = "";
        if (id == R.id.nav_my_page) {
            fragment = new UserFragment();
            title = "Моя страница";
        } else if (id == R.id.nav_family) {
            fragment = new FamilyFragment();
            title = "Семья";
        } else if (id == R.id.nav_logger) {
            fragment = new LoggerFragment();
            title = "Финансовый журнал";
        } else if (id == R.id.nav_plan) {
            fragment = new PlanFragment();
            title = "Планы";
        } else if (id == R.id.nav_history) {
            fragment = new HistoryFragment();
            title = "История";
        } else if (id == R.id.nav_setting) {
            fragment = new SettingFragment();
            title = "Настройски";
        } else if (id == R.id.nav_exit) {
            exitApp();
            return true;
        }

        if (fragment != null) {
            FragmentTransaction ft = getSupportFragmentManager().beginTransaction();
            ft.replace(R.id.content_frame, fragment);
            ft.commit();
        }

        // set the toolbar title
        if (getSupportActionBar() != null) {
            getSupportActionBar().setTitle(title);
        }

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

    private void exitApp() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<Boolean> call = RestService.getService().logoutUser();
        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Boolean isLogout = response.body();

                    if (isLogout != null && isLogout) {
                        startActivity(intent);
                    } else {
                        Toast.makeText(MenuActivity.this, "NO", Toast.LENGTH_LONG).show();
                    }
                } else {
                    Toast.makeText(MenuActivity.this, "NO", Toast.LENGTH_LONG).show();
                }

            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });

    }
}
