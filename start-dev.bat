@echo off
echo ========================================
echo    MotoTrust - Modo Desenvolvimento
echo ========================================
echo.

echo [1/2] Iniciando PostgreSQL...
docker-compose up -d postgres

echo.
echo [2/2] Aguardando PostgreSQL ficar pronto...
timeout /t 10 /nobreak > nul

echo.
echo PostgreSQL iniciado! Agora execute:
echo dotnet run --project MotoTrust.API
echo.
echo URLs disponiveis:
echo - API: http://localhost:5000
echo - Swagger: http://localhost:5000/swagger
echo - PostgreSQL: localhost:5432
echo.
echo Para parar PostgreSQL: docker-compose down
echo.
pause
