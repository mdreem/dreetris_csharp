#!/bin/bash
ROOT=$(pwd)

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
