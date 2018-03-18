package com.purse.purseclient;

import android.content.Intent;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

import android.view.View;


public class LoginActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

    }

    public void registrationView(View view) {
        Intent intent = new Intent(this, RegistrationActivity.class);

        startActivity(intent);
    }
}

