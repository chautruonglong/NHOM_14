# Generated by Django 2.2.5 on 2021-05-28 06:54

import datetime

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('core', '0012_auto_20210528_1241'),
    ]

    operations = [
        migrations.AlterField(
            model_name='student',
            name='birth',
            field=models.DateField(default=datetime.datetime(2000, 11, 9, 0, 0)),
        ),
        migrations.AlterField(
            model_name='subject',
            name='day',
            field=models.CharField(choices=[('2', 'Monday'), ('3', 'Tuesday'), ('4', 'Wednesday'), ('5', 'Thursday'), ('6', 'Friday'), ('7', 'Saturday'), ('8', 'Sunday'), (None, 'Thứ Khác')], max_length=255, null=True),
        ),
    ]
