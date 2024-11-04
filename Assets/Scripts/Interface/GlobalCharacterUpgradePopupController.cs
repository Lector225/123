using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZombieIo.Character.GlobalSkills;

public class GlobalCharacterUpgradePopupController : MonoBehaviour
{
	[SerializeField] private TMP_Text skillName;
	[SerializeField] private TMP_Text skillDescription;
	[SerializeField] private TMP_Text upgradeParameter;
	[SerializeField] private TMP_Text costText;
	[SerializeField] private Button buyButton;
	
	private GlobalSkillType type;
	private GlobalSkillData skillData;
	private int skillLevel;


	public void Initialize(GlobalSkillType type)
	{
		this.type = type;
		this.skillLevel = PlayerPrefs.GetInt("GlobalSkill_Level_" + type, skillLevel);
		
		skillData = GameManager.Instance.GlobalSkillsCollection.GetGlobalSkill(type);
		this.skillName.text = skillData.NameKey;
		this.skillDescription.text = skillData.DescriptionKey;
		costText.text = skillData.SkillCosts[skillLevel].ToString();

		buyButton.onClick.AddListener(OnClickBuyButton);
    }
	
	public void OnClickBuyButton()
	{
		if (GameManager.Instance.ScoreManager.GlobalGameScore < skillData.SkillCosts[skillLevel])
			return;

		GameManager.Instance.ScoreManager.GlobalGameScore -= skillData.SkillCosts[skillLevel];
		skillLevel++;
		PlayerPrefs.SetInt("GlobalSkill_Level_" + type, skillLevel);
		costText.text = skillData.SkillCosts[skillLevel].ToString();
	}
}
