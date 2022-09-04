@echo off
for %%i in (..) do set TARGET=%%~nxi& set PROJECT_DIR=%%~fi

@echo 正在构建发布文件...

zip -j -MM %TARGET%.zip ^
	"..\bin\Release\%TARGET%.dll"  ^
	"..\README.md" ^
	manifest.json ^
	icon.png
@echo.

if %ERRORLEVEL% NEQ 0 (
	del %TARGET%.zip
	@echo 发布文件构建失败。
) else (
	@echo 成功构建发布文件：
	dir /b %TARGET%.zip
)

@echo.
pause