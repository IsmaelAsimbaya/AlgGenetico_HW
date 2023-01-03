using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour
{
    public Text agentesTxT, comidaTxT, parasitosTxT, depreTxT, gemeTxT, viajerosTxT;

    public void setupAgente(int agenteCont){
        agentesTxT.text = agenteCont.ToString();
    }

    public void setupComida(int comidaCont){
        comidaTxT.text = comidaCont.ToString();
    }

    public void setupParasito(int paraCont){
        parasitosTxT.text = paraCont.ToString();
    }

    public void setupDepredador(int depreCont){
        depreTxT.text = depreCont.ToString();
    }

    public void setupGemelos(int gemeCont){
        gemeTxT.text = gemeCont.ToString();
    }

    public void setupViajero(int viajeCont){
        viajerosTxT.text = viajeCont.ToString();
    }

    
}
