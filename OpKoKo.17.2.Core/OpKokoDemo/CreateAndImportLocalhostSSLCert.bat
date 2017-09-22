rem From https://brockallen.com/2015/06/01/makecert-and-creating-ssl-or-signing-certificates/
makecert -r -pe -n "CN=localhost" -b 01/01/2017 -e 01/01/2027 -eku 1.3.6.1.5.5.7.3.1 -sky exchange -a sha256 -len 2048 -ss my -sr localMachine
pause