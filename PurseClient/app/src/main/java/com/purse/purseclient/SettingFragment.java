package com.purse.purseclient;


import android.app.DatePickerDialog;
import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.text.TextUtils;
import android.text.format.DateUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.purse.entity.UserData;
import com.purse.helper.UniqueFieldUser;
import com.purse.services.RestService;

import java.util.Calendar;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class SettingFragment extends Fragment implements android.view.View.OnClickListener, android.view.View.OnFocusChangeListener {
    private View view;

    private TextView tabName;
    private TextView tabCommunication;
    private TextView tabPassword;
    private TextView tabBirth;

    private LinearLayout layoutName;
    private LinearLayout layoutCommunication;
    private LinearLayout layoutPassword;
    private LinearLayout layoutBirth;

    private EditText fieldNick;
    private EditText fieldFirstName;
    private EditText fieldLastName;
    private EditText fieldPhone;
    private EditText fieldEmail;
    private EditText fieldOldPassword;
    private EditText fieldNewPassword;
    private EditText fieldConfirmPassword;
    private TextView fieldBirth;

    private Button saveSetting;

    private ProgressDialog progress;

    private UserData user;

    private Calendar dateAndTime = Calendar.getInstance();

    private boolean isUniqueNick = true;
    private boolean isUniqueEmail = true;
    private boolean isUniquePhone = true;
    private boolean isOldPassword = false;

    private int indexField;

    public SettingFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_setting, container, false);

        progress = new ProgressDialog(view.getContext());

        loadUserData();

        tabName = (TextView) view.findViewById(R.id.setting_name_form_tab);
        tabBirth = (TextView) view.findViewById(R.id.setting_date_birth_form_tab);
        tabPassword = (TextView) view.findViewById(R.id.setting_password_form_tab);
        tabCommunication = (TextView) view.findViewById(R.id.setting_communication_form_tab);

        tabName.setOnClickListener(this);
        tabBirth.setOnClickListener(this);
        tabPassword.setOnClickListener(this);
        tabCommunication.setOnClickListener(this);

        layoutCommunication = (LinearLayout) view.findViewById(R.id.setting_communication_form);
        layoutBirth = (LinearLayout) view.findViewById(R.id.setting_date_birth_form);
        layoutName = (LinearLayout) view.findViewById(R.id.setting_name_form);
        layoutPassword = (LinearLayout) view.findViewById(R.id.setting_password_form);

        fieldNick = (EditText) view.findViewById(R.id.setting_nick);
        fieldFirstName = (EditText) view.findViewById(R.id.setting_first_name);
        fieldLastName = (EditText) view.findViewById(R.id.setting_last_name);
        fieldPhone = (EditText) view.findViewById(R.id.setting_phone);
        fieldEmail = (EditText) view.findViewById(R.id.setting_email);
        fieldOldPassword = (EditText) view.findViewById(R.id.setting_old_password);
        fieldNewPassword = (EditText) view.findViewById(R.id.setting_new_password);
        fieldConfirmPassword = (EditText) view.findViewById(R.id.setting_confirm_new_password);
        fieldBirth = (TextView) view.findViewById(R.id.setting_date_birthday);

        fieldNick.setOnFocusChangeListener(this);
        fieldEmail.setOnFocusChangeListener(this);
        fieldPhone.setOnFocusChangeListener(this);
        fieldOldPassword.setOnFocusChangeListener(this);
        fieldConfirmPassword.setOnFocusChangeListener(this);

        saveSetting = (Button) view.findViewById(R.id.setting_btn);
        saveSetting.setOnClickListener(this);

        return view;
    }

    private void setDateBirth() {
        fieldBirth.setText(DateUtils.formatDateTime(view.getContext(),
                dateAndTime.getTimeInMillis(),
                DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_SHOW_YEAR));
    }

    private DatePickerDialog.OnDateSetListener d = new DatePickerDialog.OnDateSetListener() {
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
            dateAndTime.set(Calendar.YEAR, year);
            dateAndTime.set(Calendar.MONTH, monthOfYear);
            dateAndTime.set(Calendar.DAY_OF_MONTH, dayOfMonth);
            setDateBirth();
        }
    };

    public void setDate(View v) {
        new DatePickerDialog(view.getContext(), d,
                dateAndTime.get(Calendar.YEAR),
                dateAndTime.get(Calendar.MONTH),
                dateAndTime.get(Calendar.DAY_OF_MONTH))
                .show();
    }

    private void loadUserData() {
        progress.setTitle("Loading");
        progress.setMessage("Wait while loading...");
        progress.setCancelable(false);
        progress.show();

        Call<UserData> call = RestService.getService().getSessionUser();
        call.enqueue(new Callback<UserData>() {
            @Override
            public void onResponse(Call<UserData> call, Response<UserData> response) {
                progress.dismiss();
                if (response.isSuccessful()) {
                    user = response.body();
                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<UserData> call, Throwable t) {

            }
        });
    }

    @Override
    public void onClick(View v) {
        if (v == view.findViewById(R.id.setting_name_form_tab)) {
            int value = layoutName.getVisibility();
            layoutName.setVisibility(value == View.VISIBLE ? View.GONE : View.VISIBLE);
            if (value == View.GONE) {
                fieldNick.setText(user.NickName);
                fieldFirstName.setText(user.FirstName);
                fieldLastName.setText(user.LastName);
                isUniqueNick = true;
            }

        } else if (v == view.findViewById(R.id.setting_communication_form_tab)) {
            int value = layoutCommunication.getVisibility();
            layoutCommunication.setVisibility(value == View.VISIBLE ? View.GONE : View.VISIBLE);
            if (value == View.GONE) {
                fieldEmail.setText(user.Email, TextView.BufferType.EDITABLE);
                fieldPhone.setText(user.Phone, TextView.BufferType.EDITABLE);
                isUniquePhone = true;
                isUniqueEmail = true;
            }

        } else if (v == view.findViewById(R.id.setting_password_form_tab)) {
            int value = layoutPassword.getVisibility();
            layoutPassword.setVisibility(value == View.VISIBLE ? View.GONE : View.VISIBLE);
            if (value == View.GONE) {
                fieldOldPassword.setText("");
                fieldNewPassword.setText("");
                fieldConfirmPassword.setText("");
                fieldOldPassword.setEnabled(true);

                isOldPassword = false;
            }

        } else if (v == view.findViewById(R.id.setting_date_birth_form_tab)) {
            int value = layoutBirth.getVisibility();
            layoutBirth.setVisibility(value == View.VISIBLE ? View.GONE : View.VISIBLE);
            dateAndTime.setTimeInMillis(user.Birthday);
            setDateBirth();
        } else if (v == view.findViewById(R.id.setting_btn)) {
            if (isUniqueNick && isUniqueEmail && isUniquePhone) {
                boolean isChange = false;
                if (layoutName.getVisibility() == View.VISIBLE) {

                    String firstName = fieldFirstName.getText().toString();
                    if (!TextUtils.isEmpty(firstName))
                        if (!user.FirstName.equals(firstName)) {
                            user.FirstName = firstName;
                            isChange = true;
                        } else
                            fieldFirstName.setError("!");

                    String lastName = fieldLastName.getText().toString();
                    if (!TextUtils.isEmpty(lastName))
                        if (!user.LastName.equals(lastName)) {
                            user.LastName = lastName;
                            isChange = true;
                        } else
                            fieldLastName.setError("!");

                    String nick = fieldNick.getText().toString();
                    if (!TextUtils.isEmpty(nick))
                        if (!user.NickName.equals(nick)) {
                            user.NickName = nick;
                            isChange = true;
                        } else
                            fieldNick.setError("!");

                    layoutName.setVisibility(View.GONE);
                }
                if (layoutCommunication.getVisibility() == View.VISIBLE) {
                    String email = fieldEmail.getText().toString();

                    if (!TextUtils.isEmpty(email))
                        if (!user.Email.equals(email)) {
                            user.Email = email;
                            isChange = true;
                        } else
                            fieldEmail.setError("!");

                    String phone = fieldPhone.getText().toString();
                    if (!TextUtils.isEmpty(phone))
                        if (!user.Phone.equals(phone)) {
                            user.Phone = phone;
                            isChange = true;
                        } else
                            fieldPhone.setError("!");

                    layoutCommunication.setVisibility(View.GONE);
                }
                if (layoutBirth.getVisibility() == View.VISIBLE) {
                    long time = dateAndTime.getTimeInMillis();
                    layoutBirth.setVisibility(View.GONE);
                    if (user.Birthday != time) {
                        user.Birthday = time;
                        isChange = true;
                    }

                }

                if (isChange) {
                    progress.setTitle("Loading");
                    progress.setMessage("Wait while loading...");
                    progress.setCancelable(false);
                    progress.show();
                    Call<UserData> call = RestService.getService().updateUser(user);
                    call.enqueue(new Callback<UserData>() {
                        @Override
                        public void onResponse(Call<UserData> call, Response<UserData> response) {
                            progress.dismiss();
                            if (response.isSuccessful()) {
                                UserData updateUser = response.body();
                                if (updateUser != null)
                                    user = updateUser;
                                else
                                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                            } else
                                Toast.makeText(view.getContext(), "NO 1", Toast.LENGTH_LONG).show();
                        }

                        @Override
                        public void onFailure(Call<UserData> call, Throwable t) {

                        }
                    });
                }
            }
            if (isOldPassword) {
                if (layoutPassword.getVisibility() == View.VISIBLE) {

                    String newPassword = fieldNewPassword.getText().toString();
                    String confirmPassword = fieldConfirmPassword.getText().toString();

                    if (newPassword.equals(confirmPassword)) {
                        progress.setTitle("Loading");
                        progress.setMessage("Wait while loading...");
                        progress.setCancelable(false);
                        progress.show();
                        layoutPassword.setVisibility(View.GONE);
                        Call<Boolean> call = RestService.getService().updatePasswordUser(newPassword);
                        call.enqueue(new Callback<Boolean>() {
                            @Override
                            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                                progress.dismiss();
                                if (response.isSuccessful()) {
                                    Boolean ok = response.body();
                                    if (ok != null && ok)
                                        Toast.makeText(view.getContext(), "OK", Toast.LENGTH_LONG).show();
                                    else
                                        Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                                } else
                                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                            }

                            @Override
                            public void onFailure(Call<Boolean> call, Throwable t) {

                            }
                        });
                    }

                }
            }
        }
    }

    @Override
    public void onFocusChange(View v, boolean hasFocus) {
        String value;
        if (v == view.findViewById(R.id.setting_nick)) {
            value = fieldNick.getText().toString();
            if (!hasFocus && !TextUtils.isEmpty(value) && !value.equals(user.NickName)) {
                checkUniqueField(value, UniqueFieldUser.NICK_NAME);
            }
        } else if (v == view.findViewById(R.id.setting_email)) {
            value = fieldEmail.getText().toString();
            if (!hasFocus && !TextUtils.isEmpty(value) && !value.equals(user.Email)) {
                checkUniqueField(value, UniqueFieldUser.EMAIL);
            }
        } else if (v == view.findViewById(R.id.setting_phone)) {
            value = fieldPhone.getText().toString();
            if (!hasFocus && !TextUtils.isEmpty(value) && !value.equals(user.Phone)) {
                checkUniqueField(value, UniqueFieldUser.PHONE);
            }
        } else if (v == view.findViewById(R.id.setting_old_password)) {
            value = fieldOldPassword.getText().toString();
            if (!hasFocus && !TextUtils.isEmpty(value)) {
                confirmOdlPassword(value);
            }
        } else if (v == view.findViewById(R.id.setting_confirm_new_password)) {
            String password = fieldNewPassword.getText().toString();
            value = fieldConfirmPassword.getText().toString();
            if (!hasFocus && !value.equals(password)) {
                fieldConfirmPassword.setError("!");
                isUniquePhone = false;
            } else if (!hasFocus && value.equals(password)) {
                isUniquePhone = true;
            }
        }
    }

    private void checkUniqueField(String value, final int filed) {
        Call<Boolean> call = RestService.getService().uniqueField(filed, value);

        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {

                if (response.isSuccessful() && response.body() != null) {
                    if (filed == UniqueFieldUser.EMAIL) {
                        isUniqueEmail = response.body();
                        if (!isUniqueEmail)
                            fieldEmail.setError("Email is not unique!");
                    } else if (filed == UniqueFieldUser.NICK_NAME) {
                        isUniqueNick = response.body();
                        if (!isUniqueNick)
                            fieldNick.setError("Nick is not unique!");
                    } else if (filed == UniqueFieldUser.PHONE) {
                        isUniquePhone = response.body();
                        if (!isUniquePhone)
                            fieldPhone.setError("Phone is not unique!");
                    }
                } else {
                    Toast.makeText(view.getContext(), "NO", Toast.LENGTH_LONG).show();
                }

            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }

    private void confirmOdlPassword(String oldPassword) {
        Call<Boolean> call = RestService.getService().checkPassword(oldPassword);

        call.enqueue(new Callback<Boolean>() {
            @Override
            public void onResponse(Call<Boolean> call, Response<Boolean> response) {
                if (response.isSuccessful()) {
                    Boolean ok = response.body();

                    if (ok != null && !ok) {
                        isOldPassword = false;
                        fieldOldPassword.setError("Error");
                    } else {
                        isOldPassword = true;
                        fieldOldPassword.setEnabled(false);
                    }
                }
            }

            @Override
            public void onFailure(Call<Boolean> call, Throwable t) {

            }
        });
    }
}
