#!/bin/bash

dnu restore

dnu build src/Redux --configuration Release
dnu build src/Redux.DevTools.Universal --configuration Release