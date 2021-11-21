# -*- coding: utf-8 -*-
"""
Created on Sun Nov 21 20:51:52 2021

@author: Mads
"""

import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv('results2.csv')

plt.plot([i for i in range(1, len(df.index) + 1)],[float(i.split(':')[2]) + (60*float(i.split(':')[1])) for i in df[' found_time']])