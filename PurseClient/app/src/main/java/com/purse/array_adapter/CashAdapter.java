package com.purse.array_adapter;

import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.purse.entity.HistoryCash;
import com.purse.entity.Information;
import com.purse.purseclient.R;

import java.math.BigDecimal;
import java.util.Date;
import java.util.List;

public class CashAdapter  extends ArrayAdapter<HistoryCash> {

    public CashAdapter(Context context, int resource, List<HistoryCash> historyCashes) {
        super(context, resource, historyCashes);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        View v = convertView;

        if (v == null) {
            LayoutInflater inflater = (LayoutInflater) getContext().getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            v = inflater.inflate(R.layout.view_cash_entity, parent, false);
        }

        HistoryCash historyCash = getItem(position);

        if (historyCash != null) {
            TextView cashCodeField = (TextView)v.findViewById(R.id.history_cash_entity_code);
            TextView cashNameField = (TextView)v.findViewById(R.id.history_cash_entity_name);
            TextView cashBudgetField = (TextView)v.findViewById(R.id.history_cash_entity_budget);
            TextView cashDateField = (TextView)v.findViewById(R.id.history_cash_entity_date);

            cashCodeField.setText(String.valueOf(historyCash.Code));
            cashNameField.setText(historyCash.Name);
            cashBudgetField.setText(historyCash.Cash.toString());
            if (historyCash.Cash.doubleValue() < 0)
                cashBudgetField.setTextColor(Color.parseColor("#ee3535"));
            else
                cashBudgetField.setTextColor(Color.parseColor("#34c719"));

            Date date = new Date(historyCash.DateAction);
            cashDateField.setText(date.toString());
        }
        return v;
    }
}