#!/bin/bash

dnu restore

xbuild /p:Configuration=Release src/Redux.DevTools.sln

dnu build src/Redux.DevTools.Universal --configuration Release