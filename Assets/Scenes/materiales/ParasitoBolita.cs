using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasitoBolita : MonoBehaviour
{
    public Vector3 posicion;
    private float tamano = 1.0f;
    private int id = 0;
    private float velocidad = 0;
    private float rango = 0;
    private int estado = 0;
    public int agents_eat = 0;
    public float timer = 0;
    private float[] cromosoma = new float[4];

    private Material material;
    private MeshRenderer mr;

    //creo el parasito
    private BolitaManager manager;

    private AgenteBolita agenteSeleccionado;
    private Vector3 posAgente = new Vector3();

    private GameObject alimentoSeleccionado;
    private Vector3 posAlimento = new Vector3();

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        material = mr.material;
        manager = FindObjectOfType<BolitaManager>();
    }

    void Update()
    {
        posicion = transform.position;
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        timer += Time.deltaTime;

        Debug.Log(timer.ToString("f0"));

        if (timer >= 5)
        { //se queda sin tiempo muere 
            estado = 2;
        }
        if (estado == 0)
        { //buscar agente mas cercano para acercarse a el.

            float distancia = float.MaxValue;
            float d = 0;
            if (manager.agentes.Count == 0)
            {
                posAgente = new Vector3(UnityEngine.Random.Range(-50, 50),
                    UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50));
                agenteSeleccionado = null;
            }
            else foreach (AgenteBolita obj in manager.agentes)
                {
                    d = Vector3.SqrMagnitude(transform.position - obj.transform.position);
                    if (d < distancia)
                    {
                        distancia = d;
                        posAgente = obj.transform.position;
                        agenteSeleccionado = obj;
                        Debug.DrawLine(transform.position, agenteSeleccionado.transform.position);
                    }
                }
            transform.LookAt(posAgente);
            estado = 1;
        }
        if (estado == 1)
        { //perseguir agente
            //si el agente muere
            if (agenteSeleccionado == null)
            {
                estado = 0;
            }
            //verificamos si llegamos al agente
            else if (Vector3.Magnitude(transform.position - posAgente) < 0.5f)
            {
                timer = 0;
                agents_eat += 1;
                manager.agentes.Remove(agenteSeleccionado);
                //Destroy(agenteSeleccionado);
                agenteSeleccionado.PonGen(6, 10000);
                estado = 0;

                if ((agents_eat % 5) == 0)
                {
                    manager.AdicionaParasito(transform.position);
                }
            }
        }
        if (estado == 2)
        { //reaparece
            manager.AdicionaParasito(transform.position);
            manager.parasitos.Remove(this);
            GameObject.DestroyImmediate(this.gameObject);
        }
    }

    public void PonGen(int pIndice, float pValor)
    {
        cromosoma[pIndice] = pValor;
        ActualizaCromosoma();
    }

    public float ObtenGen(int pIndice)
    {
        return cromosoma[pIndice];
    }

    public void PonID(int pId)
    {
        id = pId;
    }

    private void ActualizaCromosoma()
    {
        //colocamos el tamano
        tamano = cromosoma[0];
        transform.localScale = new Vector3(tamano, tamano, tamano);
        //colocamos la velocidad
        velocidad = cromosoma[1];
        //colocamos el rando de vision
        rango = cromosoma[2];
    }
}

