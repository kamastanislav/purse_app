package com.purse.purseclient;

import android.app.ProgressDialog;
import android.content.Intent;

import android.support.annotation.BoolRes;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import com.purse.helper.Constants;
import com.purse.helper.UserLogin;
import com.purse.services.RestService;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoginActivity extends AppCompatActivity {
    private EditText fieldNick;
    private EditText fieldPassword;
    private ProgressDialog progress;
    private Intent intent;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        if (getSupportActionBar() != null) {
            getSupportActionBar().setTitle("Авторизация");
        }

        fieldNick = (EditText) findViewById(R.id.nickName);
        fieldPassword = (EditText) findViewById(R.id.password);
        progress = new ProgressDialog(this);
        intent = new Intent(this, MenuActivity.class);
    }

    public void login(View view) {
        String login = fieldNick.getText().toString();
        String password = fieldPassword.getText().toString();

        boolean hasEmptyField = changeField(login, password);
        if (hasEmptyField)
            return;

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        UserLogin userLogin = new UserLogin();
        userLogin.NickName = login;
        userLogin.Password = password;

        Call<Integer> call = RestService.getService().loginUser(userLogin);
        call.enqueue(new Callback<Integer>() {
            @Override
            public void onResponse(Call<Integer> call, Response<Integer> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Integer code = response.body();

                    if (code != null && code != Constants.DEFAULT_CODE) {
                        startActivity(intent);
                    } else {
                        Toast.makeText(LoginActivity.this, "NO", Toast.LENGTH_LONG).show();
                    }
                } else {
                    Toast.makeText(LoginActivity.this, "NO", Toast.LENGTH_LONG).show();
                }

            }

            @Override
            public void onFailure(Call<Integer> call, Throwable t) {

            }
        });

    }

    private boolean changeField(String login, String password) {
        boolean hasEmptyField = false;

        if (login.isEmpty()) {
            fieldNick.setError("Nick is required!");
            hasEmptyField = true;
        }

        if (password.isEmpty()) {
            fieldPassword.setError("Password is required!");
            hasEmptyField = true;
        }

        return hasEmptyField;
    }

    public void restorePassword(View view) {
        String login = fieldNick.getText().toString();
        String password = fieldPassword.getText().toString();

        boolean hasEmptyField = changeField(login, password);
        if (hasEmptyField)
            return;

        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        UserLogin userLogin = new UserLogin();
        userLogin.NickName = login;
        userLogin.Password = password;

        Call<Boolean> call = RestService.getService().restorePassword(userLogin);
        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    Toast.makeText(LoginActivity.this, "Новый пароль выслан на Вашу почту", Toast.LENGTH_LONG).show();
                } else {
                    Toast.makeText(LoginActivity.this, "Такого пользователя не существует", Toast.LENGTH_LONG).show();
                }

            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }

    public void registrationView(View view) {
        Intent intent = new Intent(this, RegistrationActivity.class);

        startActivity(intent);
    }
}

