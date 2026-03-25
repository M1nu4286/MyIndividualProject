@echo off
setlocal enabledelayedexpansion

set "REMOTE_URL=https://github.com/M1nu4286/MyIndividualProject.git"

:: 1. 원격 주소 재설정 (강제 업데이트)
git remote set-url origin %REMOTE_URL% >nul 2>&1
if %errorlevel% neq 0 git remote add origin %REMOTE_URL%

:: 2. 인덱스 정리 (최초 1회만 실행해도 되지만, 꼬였을 땐 필수)
echo [Cleaning Index...]
git rm -r --cached . >nul 2>&1

:: 3. 모든 파일 스테이징 (.gitignore가 쓰레기를 걸러줄 것임)
:: 루트의 .bat 파일과 기타 설정 파일까지 포함하기 위해 '.' 사용
git add .

echo.
set /p commitMsg="Enter Commit Message: "
if "%commitMsg%"=="" set "commitMsg=Refactor: Sync core assets"

:: 4. 커밋 및 푸시 (동기화 에러 방지를 위한 pull 포함)
echo [Transmitting...]
git commit -m "%commitMsg%"
git branch -M main

:: 원격에 있는 README 등과 충돌 방지
git pull origin main --rebase

:: 최종 전송
:: git push -u origin main
git push -u origin claude/player-skill-columns-p9jeM

if %errorlevel% neq 0 (
    echo.
    echo [ERROR] Push failed. Check your network or GitHub permissions.
) else (
    echo [SUCCESS] Synchronized.
)
pause