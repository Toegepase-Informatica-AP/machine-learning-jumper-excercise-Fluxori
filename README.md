# machine-learning-jumper-excercise-Phoenix
## Groepsleden
| Naam                   | S-nummer |
| ---------------------- | -------- |
| Anne Toussaint         | s106511  |
| Kirishalini Kanagarasa | s108145  |
| Yalda Fazlehaq         | s108051  |

## Inleiding
Voor het vak VR-Experience kregen wij de opdracht om een zelflerende agent aan te maken die een obstakel ontwijkt door er over te springen. Dit doen we in Unity met behulp van ML Agents.
Hierbij moest de snelheid elke episode veranderen.
Verder mochten we kiezen uit één van de volgende functionaliteiten:

* de agent wordt geconfronteerd met een rij van continu bewegende obstakels.
* de agent moet over bepaalde obstakels springen, maar andere obstakels genereren bonuspunten bij collision.
* de agent wordt ook met objecten geconfronteerd die over hem vliegen, en die moet hij vangen om bonuspunten te verdienen.
* de agent staat op het midden van een kruispunt en moet obstakels vermijden die vanuit 2 richtingen kunnen komen

Wij hebben gekozen voor de tweede optie. 

## Definities

- **Jumper**: de agent
- **Obstacle**: het obstakel waar **jumper** over moet springen
- **Coin**: de reward die **jumper** moet pakken???
- **Border**: wordt gebruikt om te detecteren dat de **coin** of **obstacle** voorbij de **jumper** is.
- **Ground**: het veld waar de **jumber**, **obstacle**, **coin** en **border** staan
- **Episode**: ???
- **Environment**: ???


## Versies

| Programma/tool | Versie         |
| -------------- | -------------- |
| Unity          | 2019.4.10      |
| Python         | 3.8.1 of hoger |
| ML Agents      | 1.0.5          |
| Tensorboard    | 2.3.1          |

## Spelverloop
Per episode zal ofwel een **obstacle** op de **jumper** afkomen, ofwel een **coin**. 
Indien het gaat om een **obstacle** moet de **jumper** erover springen en indien het gaat om een **coin** moet de **jumper** blijven staan om deze te vangen.

### Mogelijkheden beweging
| Beweging                | **Jumper** | **Obstacle** | **Coin** |
| :---------------------- | :--------: | :----------: | :------: |
| Links/Rechts (x-as)     |    Nee     |     Nee      |   Nee    |
| Springen (y-as)         |     Ja     |     Nee      |   Nee    |
| Naar de agent toe(z-as) |    Nee     |      Ja      |    Ja    |


## Obervaties, acties en beloning

De **jumper** moet onderscheid kunnen maken tussen een **obstacle** die hem bij aanraking een negatieve reward zal opleveren, en een **coin** die hem bij aanraking een positieve reward zal opleveren. Bijgevolg moet de **jumper** weten wanneer hij moet springen en wanneer niet.
Indien een **obstacle** voorbij de **jumper** gaat en de **border** aanraakt, zal de **jumper** een positieve reward krijgen.
Indien een **coin** voorbij de **jumper** gaat en de **border** aanraakt, zal de **jumper** een negatieve reward krijgen.

| Actie                               | Beloning |
| ----------------------------------- | -------- |
| **Obstacle** gaat over **border**   | + 1      |
| **Obstacle** botst tegen **jumper** | - 1      |
| **Coin** gaat over **border**       | - 1      |
| **Coin** botst tegen **jumper**     | + 1      |

## Het speelveld
Op dit speelveld staan de verschillende speelobjecten zoals **ground**, **jumper**, **obstacle**, **border**, **coin** en scoreboard. Aan de hand van deze speelobjecten hebben we ML logica opgebouwed.  

![Afbeelding speelveld](https://i.imgur.com/ppgYvq7.png)


## Spelomgeving
### **Environment** object

Alle object hebben we in een **Environment** object gestoken. Zo kunnen we het geheel later makkelijk dupliceren indien we dit willen.

![Afbeelding environment object](https://i.imgur.com/DnUMUVG.png)

Zoals te zien in de onderstaande afbeelding, heeft de **environment** ook een *Environment script* meegekregen.

![Environment](https://i.imgur.com/sbXvBtv.png)

![Environment](https://i.imgur.com/dEFM9mW.png)



### **Ground** object
Het speelveld hebben we **Ground** genoemd. Deze is een simpel vlak met schaal en rotatie:

![Afbeelding environment inspector](https://i.imgur.com/E82tbPW.png)

### **Jumper** object
De agent heet **jumper**. Deze bestaat uit een eenvoudige *cube* met schaal {XYZ: 0.5, 0.5, 0.5} en heeft een blauwe *material*.
De **jumper** heeft een *Box collider* en een *Rigidbody* component met alleen een bewegingsvrijheid in de y-as.
Bij *freeze rotation* staan alle vakjes aangevinkt, dus de **jumper** kan niet roteren.
Verder heeft de **jumper** een tag *Jumper* meegekregen.

Om ervoor te zorgen dat de acties van **jumper**
gebaseerd zijn op observaties, geven we deze een *Ray Perception Sensor 3D* component.

| Eigenschap                 | Waarde                      | Uitleg                                              |
| -------------------------- | --------------------------- | --------------------------------------------------- |
| Sensor Name                | RayPerceptionSensor         | Moet altijd uniek zijn.                             |
| Detecable Tags             | 2: **Obstacle** en **Coin** | De objecten die **jumper** kan zien                 |
| Rays Per Direction         | 0                           | Aantal stralen aan de weerzijden van de middellijn. |
| Max Ray Degrees            | 70                          | Graden tussen de verschillende stralen.             |
| Sphere Cast Radius         | 0.5                         | De dikte van elke straal                            |
| Ray Length                 | 20                          | Lengte van de stralen in dm                         |
| Stacked Raycasts           | 1                           | Geheugen voor de sensor                             |
| Start/ End Vertical Offset | 0/0                         | Naar boven of naar onder kijken                     |

![Afbeelding Jumber](https://i.imgur.com/V8G0iDC.png)



Verder voegen we nog twee componenten toe aan **jumper**: *Behavior Parameters* en *Decision Requester*. De *Behavior Parameter* zal het gedrag van de **jumper** bepalen, terwijl de *Decision Requester* de **jumper** voor een automatische trigger zal zorgen om de agent dwingen iets te doen. De warning die je ziet is normaal, omdat er nog geen Brain toegekend is aan `Model`.

![Afbeelding Behavior Parameter](https://i.imgur.com/7WkKQoS.png)

![Afbeelding Decision Requester](https://i.imgur.com/AmmOXhH.png)

### **Obstacle** object
 De **obstacle** bestaat uit een *cube* met schaal {XYZ: 0.5, 0.5, 0.5}, met een rode *material*. De **obstacle** heeft ook een passende *Box Collider*, een *Rigidbody* component met alleen een bewegingsvrijheid in de z-as met een random snelheid.
Bij *freeze rotation* staan alle vakjes aangevinkt, dus de **obstacle** kan niet roteren.
Verder heeft de **obstacle** een tag *Obstacle* meegekregen.

 ![Afbeelding Obstacle](https://i.imgur.com/AcdHKXn.png)
 
 Deze **obstacle** heeft een script genaamde [Obstacle](##-Obstacle.cs). Deze erft het script [MovingObject](##-MovingObject.cs) over. 

![Afbeelding Obstacle Inspector](https://i.imgur.com/Sa6kWte.png)

### **Coin** object
De **coin** is een *prefab* met schaal {XYZ: 0.3898, 0.3898, 0.3898}. De **coin** heeft een *Box Collider*, een *Regidbody* component met alleen een bewegingsvrijheid in de z-as met een random snelheid.
Bij *freeze rotation* staan alle vakjes aangevinkt, dus de **coin** kan niet roteren.
Verder heeft de **coin** een tag *coin* meegekregen.

 Deze **coin** heeft een script genaamde [Coin](##-Coin.cs). Deze erft ook het script [MovingObject](##-MovingObject.cs) over. 

![Afbeelding Coin](https://i.imgur.com/Td5AgEK.png)

![Afbeelding Coin Inspector](https://i.imgur.com/T31BLht.png)


### **Border** object

De **border** is een *cube* met schaal {XYZ: 3.16605, 0.5, 0.5}, met een grijze *material*. Deze kan niet bewegen of roteren.

![Afbeelding Border](https://i.imgur.com/0JYTAlV.png)

![Afbeelding Border Inspector](https://i.imgur.com/Sxyfh1S.png)


### Objects object

In de **environment** is er een leeg object genaamt Objects.
Deze gebruiken we om in de scripts mee te geven op welke plaats een **obstacle** of een **coin** geïnitialiseerd moet worden.

![Afbeelding Envoirment met object](https://i.imgur.com/dZtubAG.png)

### Scoreboard

De ScoreBoard is een instantie van de 3D-object TextMeshPro klasse dat de beloning zal weergeven.

![Afbeelding Scoreboard](https://i.imgur.com/0U4hEYN.png)

![Afbeelding Scoreboard Inspector](https://i.imgur.com/ZiL5AcJ.png)


## Scripts

### Environment.cs

`Environment` erft over van `MonoBehaviour`
```cs
public class Environment : MonoBehaviour
```
`Environment` is een beetje het "overzicht". Hierin worden alle objecten geïnitialiseerd. Vergeet niet om in de Unity GUI bij `obstaclePrefab` de **obstacle** prefab toe te voegen, en bij `coinPrefab` de **coin** prefa.
```cs
private const int MAX_SPAWNED_OBJECTS_BEFORE_END_EPISODE = 5;

public Obstacle obstaclePrefab;
public Coin coinPrefab;

internal Jumper jumper;
private TextMeshPro scoreBoard;
private GameObject objects;

private int count = 0;
```
In `OnEnable` worden de variabelen opgevuld.
```cs
public void OnEnable()
{
    objects = transform.Find("Objects").gameObject;
    scoreBoard = transform.GetComponentInChildren<TextMeshPro>();
    jumper = transform.GetComponentInChildren<Jumper>();        
}
```
De volgende methode zorgt ervoor dat het scoreboard up to date is:
```cs
private void FixedUpdate()
{
    scoreBoard.text = jumper.GetCumulativeReward().ToString("f2");
}
```
In `SpawnObject` wordt er ofwel een **obstacle** ofwel een **coin** gespawnd op de locatie van `objects`. Dit??? wordt random bepaald dankzij `int randomNumber = Random.Range(0, 2);`. Wanneer dit nummer 0 is, wordt er een **obstacle** gespawned. Wanneer dit 1 is, een **coin**. De **coin** krijgt nog een hogere y-positie mee, omdat deze anders half in de grond zit.
In het begin wordt er gecontroleerd of de count hoger/gelijk is aan `MAX_SPAWNED_OBJECTS_BEFORE_END_EPISODE`. Wanneer dit het geval is, wordt de **episode** beëindigd. Dit is om ervoor te zorgen dat een **episode** niet te lang kan duren.
```cs
public void SpawnObject()
{
    if (count >= MAX_SPAWNED_OBJECTS_BEFORE_END_EPISODE)
    {
        jumper.EndEpisode();
        return;
    }
    int randomNumber = Random.Range(0, 2);
    GameObject prefab;
    float yPos = 0;
    if (randomNumber == 0)
    {
        prefab = obstaclePrefab.gameObject;
    }
    else
    {
        prefab = coinPrefab.gameObject;
        yPos = 0.54f;
    }
    GameObject movingObject = Instantiate(prefab, new Vector3(objects.transform.position.x, objects.transform.position.y + yPos, objects.transform.position.z), Quaternion.identity);
    movingObject.transform.SetParent(objects.transform);
    count++;
}
```
`ClearEnvironment` zorgt ervoor dat de **environment** terug op zijn oorspronkelijke staat komt te staan???. `count` wordt terug op 0 gezet en alle objecten in `objects` worden verwijderd. Dit zal er momenteel maar 1 zijn, maar we doen dit toch met een `foreach`-loop zodat latere uitbreiding mogelijk is (waarin er bv. meerdere objecten worden gespawnd).
```cs
public void ClearEnvironment()
{
    count = 0;
    foreach (Transform object in objects.transform)
    {
        GameObject.Destroy(object.gameObject);
    }
}
```
### Jumper.cs

`Jumper` erft over van `Agent`.
```cs
public class Jumper : Agent
```
```cs
public float jumpForce = 5f;

private Rigidbody body;
private Environment environment;
private bool isOnGround = true;
```
In `Initialize` worden de variabelen opgevuld.
```cs
public override void Initialize()
{
    base.Initialize();
    body = GetComponent<Rigidbody>();
    environment = GetComponentInParent<Environment>();
}
```
`OnEpisodeBegin` wordt telkens als er een **episode** start aangeroepen. Hierin wordt de **environment** "opgekuist" en wordt er een nieuw object gespawnd. De velocity van **Jumper** wordt ook terug op 0 gezet, zodat deze niet nog aan het springen is als een nieuwe **episode** start.
```cs
public override void OnEpisodeBegin()
{
    environment.ClearEnvironment();
    environment.SpawnObject();
    
    body.angularVelocity = Vector3.zero;
    body.velocity = Vector3.zero;
}
```
De `Heuristic` methode wordt aangeroepen als je het spel speelt in heuristic-mode (om bv. alles even te testen voordat je de agent gaat trainen). Deze methode is redelijk simpel omdat **jumper** alleen moet kunnen springen. Er zijn dus maar 2 acties:
- Er komt geen input, dus `actionsOut[0]` wordt op 0f gezet.
- Je drukt op de `UpArrow` (pijltje naar boven) waardoor `actionsOut[0]` op 1f wordt gezet.
```cs
public override void Heuristic(float[] actionsOut)
{
    actionsOut[0] = 0f;
    
    if (Input.GetKey(KeyCode.UpArrow))
    {
        actionsOut[0] = 1f;
    }
}
```
`OnActionReceived` wordt aangeroepen wanneer er een actie gebeurd (zowel in heuristic, als wanneer de agent aan het trainen is).Ook deze methode is simpel: wanneer `vectorAction[0]` 1 is en `Jumper` is op de grond, dan wordt de methode `Jump()` aangeroepen. Wanneer we de `isOnGround`-check niet zouden doen, kan `Jumper` ook springen wanneer deze al aan het springen is, wat niet de bedoeling is.
```cs
public override void OnActionReceived(float[] vectorAction)
{
    if (vectorAction[0] == 1 && isOnGround)
    {
        Jump();
    }
}
```
`Jump` zorgt ervoor dat `Jumper` kan springen.
```cs
public void Jump()
{
    body.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    isOnGround = false;
}
```
`OnCollisionEnter` gaat af wanneer `Jumper` tegen een ander object botst. Hier wordt een negatieve reward gegeven wanneer dit een **obstacle** is, en een positieve reward wanneer dit een **coin** is.
```cs
private void OnCollisionEnter(Collision other)
{
    if (other.transform.CompareTag("Ground"))
    {
        isOnGround = true;
    }
    if (other.transform.CompareTag("Obstacle"))
    {
        AddReward(-1f);
        EndEpisode();
    }
    if (other.transform.CompareTag("Coin"))
    {
        AddReward(1f);
        Destroy(other.gameObject);
        environment.SpawnObject();
    }
}
```

### MovingObject.cs

`MovingObject` is een abstracte klasse die overerft van `MonoBehaviour`. Zowel **coin** als **obstacle** zal hiervan overerven, omdat ze zo goed als dezelfde methodes hebben.
```cs
public abstract class MovingObject : MonoBehaviour
```
```cs
private float speed = 4f;
internal Environment environment;
```
In `Start` wordt er een random speed gezet, zodat het object elke keer als deze spawnt een andere snelheid heeft.
```cs
private void Start()
{
    SetRandomSpeed();
}
```
```cs
private void FixedUpdate()
{
    Move(speed);
}
```
De `Move` methode zorgt ervoor dat het object zich met een bepaalde snelheid beweegt.
```cs
private void Move(float speed)
{
    if (environment == null)
    {
        environment = GetComponentInParent<Environment>();
    }

    Vector3 moveVector = speed * -transform.forward * Time.fixedDeltaTime;
    transform.localPosition += moveVector;
}
```
```cs
private void SetRandomSpeed()
{
    speed = Random.Range(4f, 8f);
}
```
`OnTriggerExit` is een abstracte methode omdat deze anders is bij **coin** en **obstacle**. Deze methode moet dus geïmplementeerd worden in die klassen zelf.
```cs
public abstract void OnTriggerExit(Collider other);
```

### Obstacle.cs

`Obstacle` erft over van `MovingObject` (de abstracte klasse die hierboven is uitgelegd).
```cs
public class Obstacle : MovingObject
```
De `OnTriggerExit` methode zorgt ervoor dat, wanneer `Obstacle` voorbij de **border** is, `Obstacle` verwijderd wordt en dat er een positieve reward gegeven wordt (want dit betekend dat **jumper** hierover is gesprongen).
```cs
public override void OnTriggerExit(Collider other)
{
    if (other.gameObject.CompareTag("Border"))
    {
        environment.jumper.AddReward(1f);
        environment.SpawnObject();
        Destroy(this.gameObject);
    }
}
```

### Coin.cs

Ook `Coin` erft, net als `Obstacle`, over van `MovingObject`
```cs
public class Coin : MovingObject
```
`OnTriggerExit` is bijna hetzelfde als de methode in `Obstacle`. Het enige verschil is dat er hier een negatieve reward wordt gegeven, omdat dit betekent dat **jumper** hierover is gesprongen in plaats van de `Coin` te pakken.
```cs
public override void OnTriggerExit(Collider other)
{
    if (other.gameObject.CompareTag("Border"))
    {
        environment.jumper.AddReward(-1f);
        environment.SpawnObject();
        Destroy(this.gameObject);
    }
}
```

## Testen spellogica

Om de spellogica te testen, kan je bij **jumper** in `Behavior Parameters` `Behavior Type` op *Heuristic Only* zetten.

![Afbeelding van Behavior Parameters](https://imgur.com/P5JJtxG.png)

>Vergeet niet om na het testen `Behavior Type` terug op *Default* te zetten!

## Trainen

1. Maak in je Unity project een nieuwe folder *Learning* en zet hierin het volgnde yaml bestand (met naam *jumper.yml*):

```yml
behaviors:
  Jumper:
    trainer_type: ppo
    max_steps: 5.0e5
    time_horizon: 64
    summary_freq: 10000
    keep_checkpoints: 5
    checkpoint_interval: 50000
    
    hyperparameters:
      batch_size: 32
      buffer_size: 9600
      learning_rate: 3.0e-4
      learning_rate_schedule: constant
      beta: 5.0e-3
      epsilon: 0.2
      lambd: 0.95
      num_epoch: 3

    network_settings:
      num_layers: 2
      hidden_units: 128
      normalize: false
      vis_encoder_type: simple

    reward_signals:
      extrinsic:
        strength: 1.0
        gamma: 0.99
      curiosity:
        strength: 0.02
        gamma: 0.99
        encoding_size: 256
        learning_rate : 1e-3
```

2. Voer het volgende commando uit (in de *Learning* folder): `mlagents-learn jumper.yml --run-id=Jumper01`
3. Druk op de ►-toets in de Unity GUI wanneer je volgende melding krijgt: `Listening on port 5004. Start training by pressing the Play button in the Unity Editor.`
4. Nu begin de training. Wanneer je wilt volgen hoe de training verloopt, kan je *Tensorboard* opstarten (in de *Learning* folder): `tensorboard --logdir results`
5. Nu kan je in je browser naar `http://localhost:6006` gaan. Hier zie je het dashboard met verschillende grafieken.

### Paralleliseren

Om de training sneller te laten verlopen, kan je ook meerdere **environments** in de scene zetten.

## Inspiratie

De structuur van deze Readme is gebaseerd op de cursus van meneer D'Haese.
