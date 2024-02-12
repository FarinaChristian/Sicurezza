from flask import Flask, send_file, request
import ssl
import os

app = Flask(__name__)

@app.route('/download_file', methods=['GET'])
def download_file():

    # Nome del file da scaricare
    file_path = 'payload.zip'

    # Ottieni l'indirizzo IP del client che ha effettuato la richiesta
    client_ip = request.remote_addr
    print(f"Il file è stato scaricato da: {client_ip}")

    # Invia il file al client
    return send_file(file_path, as_attachment=True)

if __name__ == '__main__':
    # Imposta l'indirizzo IP del server
    SERVER_ADDRESS = ('192.168.1.10', 12345)

    # Carica i file del certificato e della chiave privata
    ssl_context = ssl.SSLContext(ssl.PROTOCOL_TLS_SERVER)
    ssl_context.load_cert_chain(certfile='cert.pem', keyfile='key.pem')

    # Avvia il server Flask con SSL abilitato
    app.run(host=SERVER_ADDRESS[0], port=SERVER_ADDRESS[1], ssl_context=ssl_context, debug=True)
