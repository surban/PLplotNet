#$ErrorActionPreference = "Stop"

if ($env:APPVEYOR_REPO_BRANCH -ne "master") {
    Write-Host "Only uploading docs on master branch."
    exit 0
}

Add-Content "$env:USERPROFILE.git-credentials" "https://$($env:GITHUB_TOKEN):x-oauth-basic@github.com`n"
git config --global credential.helper store
git config --global user.name "Appveyor CI"
git config --global user.email "appveyor@surban.net"

git clone -b gh-pages https://github.com/surban/PLplot.git web_old
Move-Item $PSScriptRoot/Docs/_site web
Move-Item web_old/.git web

Push-Location web
git add .
git commit -m "Appveyor CI deployment"
git push
Pop-Location
