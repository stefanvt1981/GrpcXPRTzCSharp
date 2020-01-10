REM original material obtained from Stack Overflow (http://stackoverflow.com/a/37739265)
@echo off
set OPENSSL_CONF=C:\Program Files\OpenSSL-Win64\bin\openssl.cfg   

echo Generate CA key:
openssl genrsa -passout pass:1234 -des3 -out ca.key 4096

echo Generate CA certificate:
openssl req -passin pass:1234 -new -x509 -days 365 -key ca.key -out ca.crt -subj  "/C=NL/ST=ZH/L=Schiedam/O=XPRTZ/OU=GrpcXPRTzCSharp/CN=MyRootCA"

echo Generate server key:
openssl genrsa -passout pass:1234 -des3 -out server.key 4096

echo Generate server signing request:
openssl req -passin pass:1234 -new -key server.key -out server.csr -subj  "/C=NL/ST=ZH/L=Schiedam/O=XPRTZ/OU=GrpcXPRTzCSharp/CN=DESKTOP-XPRTZ"

echo Self-sign server certificate:
openssl x509 -req -passin pass:1234 -days 365 -in server.csr -CA ca.crt -CAkey ca.key -set_serial 01 -out server.crt

echo Remove passphrase from server key:
openssl rsa -passin pass:1234 -in server.key -out server.key

echo Generate client key
openssl genrsa -passout pass:1234 -des3 -out client.key 4096

echo Generate client signing request:
openssl req -passin pass:1234 -new -key client.key -out client.csr -subj  "/C=NL/ST=ZH/L=Schiedam/O=XPRTZ/OU=GrpcXPRTzCSharp/CN=DESKTOP-XPRTZ"

echo Self-sign client certificate:
openssl x509 -passin pass:1234 -req -days 365 -in client.csr -CA ca.crt -CAkey ca.key -set_serial 01 -out client.crt

echo Remove passphrase from client key:
openssl rsa -passin pass:1234 -in client.key -out client.key