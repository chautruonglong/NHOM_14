# Generated by Django 2.2.5 on 2021-05-27 18:11

import datetime

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('core', '0003_auto_20210528_0100'),
    ]

    operations = [
        migrations.AlterField(
            model_name='room',
            name='zone',
            field=models.CharField(choices=[('a', 'A'), ('b', 'B'), ('c', 'C'), ('d', 'D'), ('e', 'E'), ('f', 'F'), ('g', 'G'), ('h', 'H'), (None, 'Khu Khác')], max_length=255, null=True),
        ),
        migrations.AlterField(
            model_name='student',
            name='birth',
            field=models.DateField(default=datetime.datetime(2000, 3, 3, 0, 0)),
        ),
    ]
