# game3D
## Étapes de Collaboration

### Cloner le dépôt :
Ils ouvrent un terminal (CMD, PowerShell, ou un terminal sur Mac).
Ils exécutent la commande suivante pour cloner le dépôt :
"git clone https://github.com/Project-Group-3D/Game3D.git"
Cela crée un dossier Game3D dans leur répertoire utilisateur, qui contient tous les fichiers du projet.

### Naviguer dans le dossier cloné :
Ils doivent se déplacer dans le dossier cloné :
"cd Game3D"

### Créer une nouvelle branche :
Ils créent une nouvelle branche pour travailler sur leurs modifications, par exemple pour ajouter des assets :
"git checkout -b asset  # Ici, "asset" est le nom de la branche"

### Faire des modifications :
Ils ouvrent Unity et effectuent les modifications nécessaires (ajouter des modèles, scripts, textures, etc.).

### Ajouter les fichiers modifiés :
Une fois les modifications terminées, ils retournent au terminal et ajoutent les fichiers modifiés à l'index :
"git add .  # Cela ajoute tous les fichiers modifiés"

### Valider les modifications :
Ils effectuent une validation pour enregistrer les changements :
"git commit -m "Ajout de nouveaux assets"  # Remplacer par un message descriptif"

### Pousser la branche vers GitHub :
Ils poussent leurs changements sur la branche qu'ils ont créée :
"git push origin asset  # "asset" est le nom de leur branche"

### Créer une Pull Request :
Après avoir poussé leurs modifications, ils doivent aller sur GitHub pour créer une Pull Request afin de fusionner leurs changements dans la branche main.

### ->Comment Changer de branche
"git checkout main" ici main est le nom de la branche mais vous pouvez le changer 

### -> Comment Mettre a jour sa branche (pour avoir les dernieres modif)
"git pull origin main" ici on mets a jour la branche main

### -> Mettre a jour le depot pour voir les nouvelles branches
"git fetch --all"

### -> Voir toute les branchs
"git branch -r"

### -> Supprimer les modifications faites sur la branche actuelle
"git reset --hard"

### -> Forcer la maj des references distanciels
"git fetch --prune"

### Ensuite on merge la branche sur laquelle les modifications on été faites sur la branche principale "main" pour avoir le code au complet
