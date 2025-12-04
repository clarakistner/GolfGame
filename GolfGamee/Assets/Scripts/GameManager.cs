using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    [Header("Referências de UI")]
    public TMP_Text textTacadas;
    public TMP_Text textPar;
    public TMP_Text textRecorde;
    public TMP_Text textResumoFinal;

    [Header("Configuração da Fase")]
    public int par;
    public string idFase = "fase01"; // ID único por fase

    [Header("Configurações de Jogo")]
    public int maxTacadas = 5;
    public GameObject painelGameOver; // Painel com texto e botão de reinício

    [Header("Painel de Finalização")]
    public GameObject painelFinalizacao;
    public TMP_Text textParabens;
    public TMP_Text textRecordeFinal;

    private int tacadas;
    private int recorde;

    [Header("Avanço de Fase")]
public string nomeProximaFase = "nomeDaCena"; // preencha no Inspector

public void CarregarProximaFase()
{
    if (!string.IsNullOrEmpty(nomeProximaFase))
    {
        SceneManager.LoadScene(nomeProximaFase);
    }
    else
    {
        Debug.LogWarning("Nome da próxima fase não está atribuído!");
    }
}


    void Start()
    {
        if (gm == null)
            gm = this;

        tacadas = 0;
        recorde = PlayerPrefs.GetInt("Recorde_" + idFase, int.MaxValue);

        AtualizarUI();

        if (painelGameOver != null) painelGameOver.SetActive(false);
        if (painelFinalizacao != null) painelFinalizacao.SetActive(false);
    }

    public void Tacada()
    {
        if (!PodeJogar()) return;

        tacadas++;
        AtualizarUI();

        if (tacadas >= maxTacadas)
        {
            MostrarGameOver();
        }
    }

    void AtualizarUI()
    {
        if (textTacadas != null)
            textTacadas.text = "Tacadas: " + tacadas;

        if (textPar != null)
            textPar.text = "Par: " + par;

        if (textRecorde != null)
            textRecorde.text = "Recorde: " + (recorde == int.MaxValue ? "--" : recorde.ToString());
    }

    public void FinalizarFase()
    {
        VerificarRecorde();

        if (textResumoFinal != null)
            textResumoFinal.text = "Você fez: " + NomePontuacao();

        AtualizarUI();
    }

    public void MostrarFinalizacao()
    {
        if (painelFinalizacao != null)
        {
            painelFinalizacao.SetActive(true);

            if (textParabens != null)
                textParabens.text = "Parabéns!";

            if (textResumoFinal != null)
                textResumoFinal.text = "Você fez: " + NomePontuacao();

            if (textRecordeFinal != null)
                textRecordeFinal.text = "Recorde: " + (recorde == int.MaxValue ? "--" : recorde.ToString());
        }
    }
    void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Bola"))
    {
        GameManager.gm.FinalizarFase();
        GameManager.gm.MostrarFinalizacao();
    }
}

    
Vector3 ultimaPosicaoValida;

void Update()
{
    
     MonitorarBola();
}
void MonitorarBola()
{
    GameObject bola = GameObject.FindWithTag("Bola");
    if (bola != null)
    {
        Rigidbody rb = bola.GetComponent<Rigidbody>();

        if (rb.velocity.magnitude < 0.05f)
        {
            ultimaPosicaoValida = bola.transform.position;
        }

        if (bola.transform.position.y < -2f) // Caiu da pista
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            bola.transform.position = ultimaPosicaoValida;
        }
    }
}



    void VerificarRecorde()
    {
        if (tacadas < recorde)
        {
            recorde = tacadas;
            PlayerPrefs.SetInt("Recorde_" + idFase, recorde);
        }
    }

    string NomePontuacao()
    {
        int diff = tacadas - par;
        if (diff <= -3) return "Albatross";
        if (diff == -2) return "Eagle";
        if (diff == -1) return "Birdie";
        if (diff == 0) return "Par";
        if (diff == 1) return "Bogey";
        return "Double Bogey+";
    }

    bool PodeJogar()
    {
        GameObject bola = GameObject.FindWithTag("Bola");
        if (bola == null) return false;

        Rigidbody rb = bola.GetComponent<Rigidbody>();
        return rb != null && rb.velocity.sqrMagnitude < 0.0025f;
    }

    void MostrarGameOver()
    {
        if (painelGameOver != null)
            painelGameOver.SetActive(true);
    }

    public void ReiniciarFase()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrParaCena(string nomeDaCena)
{
    SceneManager.LoadScene(nomeDaCena);
}
}