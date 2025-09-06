@echo off
echo ========================================
echo    MotoTrust - Iniciando com Docker
echo ========================================
echo.

echo [1/3] Parando containers existentes...
docker-compose down

echo.
echo [2/3] Construindo e iniciando containers...
docker-compose up --build

echo.
echo [3/3] Aplicacao iniciada!
echo.
echo URLs disponiveis:
echo - API: http://localhost:5000
echo - Swagger: http://localhost:5000/swagger
echo - PostgreSQL: localhost:5432
echo.
echo Para parar: docker-compose down
echo Para ver logs: docker-compose logs -f
echo.
pause
