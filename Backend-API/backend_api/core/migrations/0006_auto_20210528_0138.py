# Generated by Django 2.2.5 on 2021-05-27 18:38

import datetime

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('core', '0005_auto_20210528_0138'),
    ]

    operations = [
        migrations.AlterField(
            model_name='student',
            name='birth',
            field=models.DateField(default=datetime.datetime(2000, 4, 16, 0, 0)),
        ),
    ]
