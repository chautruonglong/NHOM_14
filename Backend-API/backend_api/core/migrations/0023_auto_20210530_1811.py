# Generated by Django 2.2.5 on 2021-05-30 11:11

import datetime

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('core', '0022_auto_20210530_1803'),
    ]

    operations = [
        migrations.AlterField(
            model_name='lecturer',
            name='faculty',
            field=models.CharField(max_length=1023, null=True),
        ),
        migrations.AlterField(
            model_name='student',
            name='birth',
            field=models.DateField(default=datetime.datetime(2000, 8, 27, 0, 0)),
        ),
        migrations.AlterField(
            model_name='subject',
            name='day',
            field=models.CharField(max_length=255, null=True),
        ),
    ]