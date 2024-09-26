Per testare le API utilizzando lo swagger, prima è necessario seguire questi passaggi:

1. Lanciare in Debug la soluzione, all'URL: https://localhost:7290/swagger/index.html
2. Autenticarsi utilizzando la funzione /Auth/login e l'account admin:
   - Username: admin
   - Password: admin
3. La chiamata restituirà un token, copiarlo e salvarlo da qualche parte.
4. Premere il tasto Authorize in alto a destra all'interno dello swagger e incollare il token nel formato: Bearer {token}. (per l'utente admin basterà incollare il seguente: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE3MjczMDI2NzMsImV4cCI6MTcyNzMwNjI3MywiaWF0IjoxNzI3MzAyNjczfQ.HyxMp3vDm5mmQ4MewIkWzAydEzxGNuoU9nb8NlN8ahU )
5. Ora sarà possibile utilizzare le altre funzioni come utenti autenticati.
