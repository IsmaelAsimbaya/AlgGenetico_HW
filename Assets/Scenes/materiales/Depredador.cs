using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depredador : MonoBehaviour
{
    public Vector3 posicion;
    public float energiaPareja = 6000;
    private float[] cromosoma = new float[7];
    //identificacion de los agentes
    private int id = 0;
    private int estado = 0;
    public float energia = 1500;
    private Vector3 pareja;
    private float tamano = 1.0f;
    private float velocidad = 0;
    private float rango = 0;
    private float costo = 1.5f;

    //cambia el color de la bolita
    private Material material;
    private MeshRenderer mr;

    private BolitaManager manager;

    private AgenteBolita alimentoAgenteSeleccionado;
    private Vector3 posAlimento = new Vector3();

    private Depredador parejaSeleccionada;
    private Vector3 posPareja = new Vector3();

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        material = mr.material;
        manager = FindObjectOfType<BolitaManager>();
    }

    void Update()
    {
        posicion = transform.position;
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        //actualizamos la energia
        energia -= costo;
        if (energia <= 0) //verificamos si muere
        {
            estado = 2;
        }
        if (estado == 0) //buscar alimento selecciona el alimento mas cercano para acercarse a el.
        {
            float distancia = float.MaxValue;
            float d = 0;
            if (manager.agentes.Count == 0)
            {
                posAlimento = new Vector3(UnityEngine.Random.Range(-50, 50),
                    UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-50, 50));
                alimentoAgenteSeleccionado = null;
            }
            else foreach (AgenteBolita agente in manager.agentes)
                {
                    d = Vector3.SqrMagnitude(transform.position - agente.transform.position);
                    if (d < distancia)
                    {
                        distancia = d;
                        posAlimento = agente.transform.position;
                        alimentoAgenteSeleccionado = agente;
                    }
                }
            transform.LookAt(posAlimento);
            estado = 1;
        }
        if (estado == 1) //perseguir alimento
        {
            //si el alimento ya fue tomado
            if (alimentoAgenteSeleccionado == null)
            {
                estado = 0;
            }
            //verificamos si llegamos al alimento
            else if (Vector3.Magnitude(transform.position - posAlimento) < 0.5f)
            {
                energia += (alimentoAgenteSeleccionado.GetComponent<AgenteBolita>().energia)/2;
                //manager.AdicionaAlimento(transform.position);
                alimentoAgenteSeleccionado.energia = 0;
                //manager.agentes.Remove(alimentoAgenteSeleccionado);
                //Destroy(alimentoAgenteSeleccionado);
                
                estado = 0;

                if (energia > energiaPareja)
                {
                    estado = 3;
                }
            }
        }
        if (estado == 2) //muerto
        {
            //manager.AdicionaAlimento(transform.position);
            manager.depredadores.Remove(this);
            Depredador.DestroyImmediate(this.gameObject);
        }
        if (estado == 3) //buscaPareja
        {
            float distancia = float.MaxValue;
            float d = 0;
            parejaSeleccionada = null;

            foreach (Depredador a in manager.depredadores)
            {
                if (a.energia >= energiaPareja && a.id != id)
                {
                    d = Vector3.Magnitude(a.transform.position - transform.position);
                    if (d < distancia && d < cromosoma[5])
                    {
                        distancia = d;
                        parejaSeleccionada = a;
                    }
                }
            }
            //verificamos que encontro pareja
            if (parejaSeleccionada == null)
            {
                estado = 0;
            }
            else
            {
                estado = 4;
            }
        }
        if (estado == 4)
        { //persigue pareja
            if (parejaSeleccionada == null)
                estado = 0;
            else
            {

                posPareja = parejaSeleccionada.posicion;
                transform.LookAt(posPareja);

                Debug.DrawLine(transform.position, parejaSeleccionada.transform.position);
                if (Vector3.Magnitude(posPareja - transform.position) <
                (cromosoma[0] + parejaSeleccionada.ObtenGen(0) + 1.5))
                {
                    estado = 5;
                }
            }
            if (energia <= 0) estado = 2;
            else if (energia < 500) estado = 0;
        }
        if (estado == 5)
        { //reproduccion
            manager.CruceDepred(this, parejaSeleccionada);
            estado = 0;
        }
    }
    //coloca valor dentro del cromosoma.
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

        //colocamos el color
        Color temp = new Color(cromosoma[1], cromosoma[2], cromosoma[3]);
        material.color = temp;

        //colocamos la velocidad
        velocidad = cromosoma[4];

        //colocamos el rando de vision
        rango = cromosoma[5];

        //colocamos el costo
        costo = cromosoma[6];

    }
}
