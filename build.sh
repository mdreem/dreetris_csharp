#!/bin/bash
ROOT=$(pwd)

MONOGAME=/Library/Frameworks/Mono.framework/External/xbuild/MonoGame/v3.0/

cp ${MONOGAME}/Assemblies/DesktopGL/libSDL2-2.0.0.dylib ${ROOT}/Dreetris
cp ${MONOGAME}/Assemblies/DesktopGL/libopenal.1.dylib ${ROOT}/Dreetris
cp ${MONOGAME}/Assemblies/DesktopGL/MonoGame.Framework.dll.config ${ROOT}/Dreetris
cp -r ${MONOGAME}/Assemblies/DesktopGL/x64 ${ROOT}/Dreetris
cp -r ${MONOGAME}/Assemblies/DesktopGL/x86 ${ROOT}/Dreetris

chmod 644 ${ROOT}/Dreetris/libSDL2-2.0.0.dylib
chmod 644 ${ROOT}/Dreetris/libopenal.1.dylib
chmod 644 ${ROOT}/Dreetris/MonoGame.Framework.dll.config
chmod 644 ${ROOT}/Dreetris/x64/*
chmod 644 ${ROOT}/Dreetris/x86/*

msbuild ./Dreetris/Dreetris.csproj /p:Configuration=Release /v:d /t:Build

MGCB_DIR=/Library/Frameworks/Mono.framework/External/xbuild/MonoGame/v3.0/Tools
MGCB=${MGCB_DIR}/MGCB.exe

cd ${ROOT}
mkdir -p ${ROOT}/Dreetris/bin/tmp
cp ${ROOT}/Dreetris/Content/music.ogg ${ROOT}/Dreetris/bin/tmp

pushd ${MGCB_DIR}
mono ${MGCB} /platform:Linux /build:${ROOT}/Dreetris/bin/tmp/music.ogg
popd

cp ${ROOT}/Dreetris/bin/tmp/music.ogg ${ROOT}/Dreetris/bin/Release/Content
cp ${ROOT}/Dreetris/bin/tmp/music.xnb ${ROOT}/Dreetris/bin/Release/Content
