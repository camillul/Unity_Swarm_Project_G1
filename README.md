# Modélisation Essaim de drône.
***
Ce projet vise à créer/simuler un essaim de drône sur Unity.
Ces essaims de drône peuvent avoir différente mise en situation tels que la défense, le civile, le secourisme et est un domaine de plus en plus répandu.


## Table of Contents

1. [Introduction : Intelligence distribuée]
2. [Exemple : Boid]
3. [Projet]


### Introduction : Intelligence distribuée : 


Un intelligence distribué est un système d'interaction entre agents/élément dans l'accomplissement d'une tâche ou d'un déplacement 
.Ces agents sont souvent de même nature (mais pas nécéssairement, ils peuvent être hétérogène) et constitue une population de n'importe quelle échelle (3, 10 , 100, 10000 ,.....).
Il est important de ne pas la confondre avec une "flotte" qui elle ,possède un capitaine et des vaisseaux qui suivent ce capitaine. Contrairement à l'intelligence distribué qui elle est "robuste". 
Chaque drône considère son voisinage direct pour "comprendre" la situation et s'adapter.ainsi aucune station au sol n'est requise afin de donner des commandes au drône, celui-ci improvise (comme un musicien qui rejoindrait un groupe de musique n,il communique/ecoute juste le BPM et la clé du morceau envirronnante et il peut s'intégrér ^_^).




### Exemple : Boid

La méthode des Boids permet de créer une intelligence distribué pour contrôler les mouvements d'un essaim. Celle-ci repose sur 3 principes :
	- Cohésion : permet de faire converger les agents vers une position moyenne une position moyenne elle même calculée selon les agents voisins.
	
	-Séparation : permet de maintenir une distance entre chaque agent, comme un effet de réplusion les uns entre les autres
	
	
	-Alignement : permet de faire converger l'angle/la direction plus ou moins rapidement vers la direction moyenne de tout les agents alentours.
	

En pratique, ces règles peuvent renvoyer un vecteur consigne à appliquer à l'agents 
la somme de ces 3 vecteur feront converger les agents vers une position "optimale" qui repondra au mieux aux 3 critères. Ces règles peuvent être coefficientés afin d'en augmenter ou diminuer les effets. Ainsi on se retrouve avec :

Vconsigne = a* Règle1 + b* Règle2 + c* Règle3

avec a,b,c des réels poisitifs ou nul.



### Projet
Nous avons ici essayer d'appliquer l'intelligence distribué avec un essaim en 3 dimension sous Unity afin de le rentre plus interactif et nottament développer derrière plusieur type de scénarion.

#### Mise en situation

Dans ce projet nous avons une mise en situation dde plusieur essaims dans le cadre d'une course poursuite.
Si le véhicule contrôlé par l'utilisateur dépasse la vitesse de 90 km/h alors des drône en essaim se lance à sa poursuite avec constament des drônes le long de la route qui rediffuse la position de la voiture (si possible).

Une fois le projet terminée, les drône de surveillance routière s'adapterons à leur nombre pour recouvrir un maximum la route et les drône offensif auront une commande d'aligner 

#### Contrôle 
Voiture :
[cross (Left, Rigth, Up, Down)] : permet de contrôlé le véhicule en accélération et direction
[Space] : permet de freiner le véhicule.

Drone :
[O] permet de rendre autonôme ou non l'essaim de drône
[I,J,K,L] : Permet de contrôler l'essaim de drône lorsque celui-ci n'est pas autonome sur les axes (x,y)
[Z,S] : Permet de contrôler l'essaim de drône et son altitude.

#### Structure


Les principaux élément dans la scène sont

	-L'UI

	-La voiture 

	-Les essaims

	-L'agent

L'UI permet simplement d'avoir un début d'interface qui permettra de gerer la scène/la simulation. le script peut être porter par un gameObject vide par unity mais de préférence nous aurons une hiérarchie ou l'UI sera au dessus de tout les autre gameObject ou bien référencer le gameObject depuis l'éditeur l'objet "Swarm" possèdant tout les agents pour y avoir accès et changer leur paramètres si nécéssaire car si l'on demande à Unity un gameObject qui n'est pas dans sa hiérarchie celui-ci éxécute une recherche avec un algorithmme de graphe (recherche opération, qui peut être très vite être gourmande en ressource si jamais on a beaucouo de gameObject (par exemple 1000000 agents ...).
Dans notre cas il gère le nombre d'agents  à créer, la vitesse des agents, et les coefficient a,b,c des 3 règles

La voiture est elle programmé de tel sorte que chaque roue est indépendante et permet de calculer tout la physique de déplacement en prenant en compte les frottements, le couple moteur et d'autres paramètres.
Certains détails

Ainsi Le gameObject Essaim est aussi est encore un gameObject vide mais permettra, de regrouper les agents et gérer leurs physiques ensemble.
Elle est l'équivalente de la station au sol sauf que celle-ci ne commande en rien les drône, elle est juste interatif avec eux dans le cadre de notre  simulation afin de pouvoir changer leur vitesse et coefficient.
Le type d'essaim est défini par le script qu'il contient, dans notre cas il y en a deux : Swarm_target et Swarm_eye.
Swarm_target donne à tout ces drône enfant la cible qu'ils doivent poursuivre et donner la consigne sous forme d'un vecteur qui s'ajoutera au vecteur résultat comportant déjà la commande de comportement en boid.

Enfin les Agents, le gameObject "réel" avec un mesh (on peut sélectionner n'importe quels agent/forme en changeant le mesh).
Celui-ci possède deux script, un script qui va SEULEMENT calculer le vecteur consigne et  un autre qui va s'occuper de réaliser/mettre à jour les mouvements selon le vecteur consigne et aussi choisir le comportement/la consigne qu'il veut executer.En effet, il faut prendre en compte la possibilité que l'agent puisse se retrouver tout seul ainsi impossible de calculer une consigne par les regles de Boid.


À partir de là, plusieurs problématique sont apparut (propre à Unity et pas forcément à celui d'un système réel, même si celle-ci peuvent être similaires)

-La recherche de voisinage, qui si l'on demande à tout les robots :"es-tu dans mon voisinage ?" on obtient un complexité en (O)n²

-L'esquive d'obstacle, qui est nécéssaire pour ne pas foner dans le murs et pas forcement simple à implmenter si on veut considérer le chemin optimal pour esquiver un objet.

-L'utilisation excessive de la physique dans la scène, celle doit être utilisé le moins possible car plus gourmande contrairement à juste changer la transform d'un gameObject.
 

Collider

 la parti recherche de voisinage afin de réduire la complexité, une approche connu (sur d'autre moteur 3d, ou simulation), est la discrétisation de l'espace et se réduire à l'étude du voisinage seulement dans les cases à proximité de celle échantilloné. ceci à une compléxité en (O)log(n)
Ma solution implémentable seulement via un moteur tels que Unity est la suivante : Utilsier les Colliders de Unity afin de mettre à jour le nombre de voisin autour de l'agent.
Les Colliders sont tout d'abord un des élément utilisé pour la physique de Unity celui-ci. Il permet de créer des collisions entre des objects.

/ ! \ si on veut une "vrai" collision un component "Rigidbody" est nécéssaire.
Il y a tout de même quelque subtilités nottament le mode Trigger que nous utiliserons afin de detecter un autre gameObject/Collider avec la fonction OnTriggerEnter(), qui s'active lorsque l'on a une collision (une fonction intégrer par Unity au même titre que Start(), Update(),....
OnTriggerExit() est aussi utilsié pour les sortie de chaque GameObject. 
Ainsi a chaque agent aura une liste de voisin qui sera déterminé par la taille du collider de "detection" et ainsi avoir un complixité surement proche du (O)log(n). bien sur dans cette hypothèse je néglige la complexité induite par OnTriggerEnter/Exit(), cependant elle reste une fonction très bien optimisé ! 





