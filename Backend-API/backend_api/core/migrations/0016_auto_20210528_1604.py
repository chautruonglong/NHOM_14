# Generated by Django 2.2.5 on 2021-05-28 09:04

import datetime

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('core', '0015_auto_20210528_1407'),
    ]

    operations = [
        migrations.AlterField(
            model_name='student',
            name='birth',
            field=models.DateField(default=datetime.datetime(2000, 11, 11, 0, 0)),
        ),
    ]