-Le cartelle esame ed esameServer non servono perchè sono le prime versioni in visualstudio che avevo testato

-Report_MalDoc.docx è la relazione

-client.cs è la console da avviare su kali, all'inizio vi chiederà di inserire l'ip della vittima (fornito dal server python). L'unico comando malevolo che potete inviare è MESSAGGIO>>>scriviquellochevuoi. Per eseguirlo ho usato Mono.

-myDoc.docm è il word offuscato, non vi serve a nulla perchè dovete cambiare l'ip e il percorso dove volete salvare il virus, quindi buttatelo

-prova.docm è il word malevolo, dentro dovete cambiare l'ip (mettete quello della macchina kali) e il percorso dove volete salvare il virus. Nel word i comandi non sono direttamente in base 64, li ho prima convertiti e poi riconvertiti per far vedere alla prof come facevo. Voi metteteli direttamente in base 64, c'è un metodo nella macro che vi converte le stringhe, fatevele stampare e mettete il comando codificato direttamente nei decodificatori

-payload.exe è il virus per windows, non dovete fare nulla.

-server.cs è il codice del payload, non vi serve (vi serve in caso voleste dare un'occhiata alla struttura del virus)

-server.py è il programma python che permette di usare https. Dovrete creare un certificato autogenerato. Quando la vittima scaricherà il virus, questo server vi stamperà il suo ip. Anche qui dovete cambiare l'ip.
