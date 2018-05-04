package com.purse.array_adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.purse.entity.Information;
import com.purse.purseclient.R;

import java.util.List;

public class HistoryAdapter extends ArrayAdapter<Information> {

    public HistoryAdapter(Context context, int resource, List<Information> informations) {
        super(context, resource, informations);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        View v = convertView;

        if (v == null) {
            LayoutInflater inflater = (LayoutInflater) getContext().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            v = inflater.inflate(R.layout.view_history_entity, parent, false);
        }

        Information information = getItem(position);

        if (information != null) {
            TextView historyEntityName = (TextView) v.findViewById(R.id.history_entity_name);
            historyEntityName.setText(information.Info);

            TextView historyPlan = (TextView) v.findViewById(R.id.history_plan_code);
            historyPlan.setText(String.valueOf(information.PlanCode));
        }
        return v;
    }
}