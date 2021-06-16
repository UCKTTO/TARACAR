using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using Firebase;
using Firebase.Database;

public class HomepageManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtUser, txtUsersDaily, txtUsersWeekly, txtUsersMonthly;
    [SerializeField]
    private List<GameObject> adminStuff;
    private DatabaseReference reference;
    private int uD, uW, uM;
    private bool hasLoaded;

    private void Start() {
        txtUser.text = GlobalData.userID;
        foreach (GameObject go in adminStuff)
        {
            go.SetActive(GlobalData.isAdmin);
        }

        // FirebaseDatabase.DefaultInstance.GetReferenceFromUrl("https://taracar-7b824-default-rtdb.asia-southeast1.firebasedatabase.app/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        UpdateUsersCount();
    }

    public void UpdateUsersCountText() {
        txtUsersDaily.text = uD.ToString();
        txtUsersWeekly.text = uW.ToString();
        txtUsersMonthly.text = uM.ToString();
    }

    public void LogOut() {
        SceneManager.LoadScene("1");
    }

    private void UpdateUsersCount() {
        hasLoaded = false;
        StartCoroutine(SaveData());
        FirebaseDatabase.DefaultInstance
            .GetReference("usersCount")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    // Handle the error...
                }
                else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    uD = int.Parse(snapshot.Child("daily").Value.ToString()) + 1;
                    uW = int.Parse(snapshot.Child("weekly").Value.ToString()) + 1;
                    uM = int.Parse(snapshot.Child("monthly").Value.ToString()) + 1;
                    hasLoaded = true;
                }
            });
    }
    private IEnumerator SaveData() {
        yield return new WaitUntil( () => hasLoaded );
        reference.Child("usersCount").Child("daily").SetValueAsync(uD);
        reference.Child("usersCount").Child("weekly").SetValueAsync(uW);
        reference.Child("usersCount").Child("monthly").SetValueAsync(uM);
    }
}