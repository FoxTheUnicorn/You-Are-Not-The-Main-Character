using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacter : NPCCharacter
{
    // Start is called before the first frame update
    public override void Start()
    {
        maxHealth = 1000;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.Update();
    }
}
