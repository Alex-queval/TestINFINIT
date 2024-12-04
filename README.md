# Créer un token GitHub et l'ajouter dans `appsettings.json`

## Étape 1 : Créer un token d'accès personnel sur GitHub

1. **Connectez-vous à GitHub** :
   Allez sur [GitHub](https://github.com) et connectez-vous avec vos identifiants.

2. **Accédez aux paramètres du compte** :
   Cliquez sur votre photo de profil en haut à droite, puis sélectionnez **Settings** (Paramètres).

3. **Générez un token** :
   - Dans le menu de gauche, cliquez sur **Developer settings** (Paramètres développeur).
   - Sélectionnez **Personal access tokens** (Tokens d'accès personnel).
   - Cliquez sur **Generate new token** (Générer un nouveau token).
   - Donnez un nom à votre token et sélectionnez les autorisations nécessaires en fonction de ce que vous souhaitez faire (par exemple, `repo`, `workflow`, etc.).
   - Cliquez sur **Generate token** en bas de la page.

4. **Copiez votre token** :
   Une fois généré, **copiez immédiatement le token**, car vous ne pourrez plus le voir après avoir quitté la page.

## Étape 2 : Ajouter le token dans le fichier `appsettings.json`

1. **Ouvrez votre fichier `TestINFINIT/appsettings.json`** dans votre projet.

2. **Ajoutez le token dans le fichier** :
   Ajoutez une section pour stocker le token. Par exemple :

   ```json
   {
     "GitHubConnectionConfigs": {
       "Token": "Your GitHub token"
     }
   }
