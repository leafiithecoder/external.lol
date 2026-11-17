@echo off

git add .
set /p "COMMIT_NAME=commit name: "
git commit -m "%COMMIT_NAME%"
git push