package com.purse.array_adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.purse.entity.UserData;
import com.purse.purseclient.R;

import java.util.List;

public class UserAdapter extends ArrayAdapter<UserData> {

    public UserAdapter(Context context, int resource, List<UserData> userDatas) {
        super(context, resource, userDatas);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        View v = convertView;

        if (v == null) {
            LayoutInflater inflater = (LayoutInflater) getContext().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            v = inflater.inflate(R.layout.view_user_entity, parent, false);
        }

        UserData userData = getItem(position);

        if (userData != null) {
            TextView fieldName = (TextView) v.findViewById(R.id.user_entity_name);
            TextView fieldCode = (TextView) v.findViewById(R.id.user_entity_code);
            TextView fieldCash = (TextView) v.findViewById(R.id.user_entity_cash);
            fieldName.setText(String.format("%1$s %2$s (%3$s)", userData.FirstName, userData.LastName, userData.NickName));
            fieldCode.setText(String.valueOf(userData.Code));
            fieldCash.setText(userData.Cash.toString());
        }

        return v;
    }
}