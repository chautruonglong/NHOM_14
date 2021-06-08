# Generated by Django 2.2.5 on 2021-05-30 09:25

import datetime

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('core', '0017_auto_20210530_1402'),
    ]

    operations = [
        migrations.AddField(
            model_name='student',
            name='class_name',
            field=models.CharField(max_length=255, null=True),
        ),
        migrations.AlterField(
            model_name='student',
            name='birth',
            field=models.DateField(default=datetime.datetime(2000, 4, 26, 0, 0)),
        ),
    ]