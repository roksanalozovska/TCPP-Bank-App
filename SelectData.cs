using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Office.Interop.Word;
using Word= Microsoft.Office.Interop.Word;

namespace BankClients
{
    public class SelectData
    {
        public List<Client> clientList = new List<Client>();
        public List<Client> selectedNameList = new List<Client>();    // Результат для X
        public List<Client> selectedXYList = new List<Client>();      // Результат для XY
        // Ініціалізація змінних для роботи з Word
        private Word.Application wordApp;
        private Word.Document wordDoc;
        private string filePath;

        // Реалізація Критерію Х (Відбір за іменем)
        public void SelectX(string clientName)
        {
            selectedNameList.Clear();
            foreach (Client client in clientList)
            {
                if (client.name == clientName)
                {
                    selectedNameList.Add(client);
                }
            }
        }

        // Реалізація Критерію XY (фільтрація за сумою)
        public void SelectXY(double minBalance)
        {
            selectedXYList.Clear();
            foreach (Client client in clientList)
            {
                if (client.balance > minBalance)
                {
                    selectedXYList.Add(client);
                }
            }
        }
        // Пошук текстових міток у Word та їх заміна на наші дані 
        private void ReplaceText(string replacedText, string textToReplace)
        {
            try
            {
                Word.Range selText = wordDoc.Range(wordDoc.Content.Start, wordDoc.Content.End);
                Word.Find find = wordApp.Selection.Find; 

                find.Text = replacedText; 
                find.Replacement.Text = textToReplace; 

                object wrap = Word.WdFindWrap.wdFindContinue;
                object replace = Word.WdReplace.wdReplaceAll;  

                find.Execute(FindText: Type.Missing, MatchCase: false, MatchWholeWord: false,
                             MatchWildcards: false, MatchSoundsLike: Type.Missing, MatchAllWordForms: false,
                             Forward: true, Wrap: wrap, Format: false, ReplaceWith: Type.Missing, Replace: replace);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка заміни тексту: " + ex.Message);
            }
        }

        // Перевизначений метод для заповнення таблиць у Word
        private void ReplaceText(List<Client> selectedList, int numTable)
        {
            try
            {
                for (int i = 0; i < selectedList.Count; i++)
                {
                    wordDoc.Tables[numTable].Rows.Add();
                    wordDoc.Tables[numTable].Cell(2 + i, 1).Range.Text = selectedList[i].fullName;
                    wordDoc.Tables[numTable].Cell(2 + i, 2).Range.Text = selectedList[i].age.ToString();
                    wordDoc.Tables[numTable].Cell(2 + i, 3).Range.Text = selectedList[i].balance.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка заповнення таблиці Word: " + ex.Message);
            }
        }

        // Створення документа на основі шаблону 
        public void WriteData(string searchName, string searchYears, string searchSum)
        {
            filePath = Environment.CurrentDirectory.ToString();

            try
            {
                wordApp = new Word.Application(); 
                wordDoc = wordApp.Documents.Add(filePath + "\\Шаблон_Банк.dot"); 
                wordApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13) +
                    "Помістіть файл Шаблон_Банк.dot у каталог із ехе-файлом програми і повторіть збереження",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Вставляємо критерії пошуку в текст шаблону 
            ReplaceText("[X]", searchName);
            ReplaceText("[Years]", searchYears);
            ReplaceText("[Sum]", searchSum);
            // Заповнюємо таблиці у Word
            if (selectedNameList.Count > 0) ReplaceText(selectedNameList, 1);
            if (selectedXYList.Count > 0) ReplaceText(selectedXYList, 2);

            try
            {
                wordDoc.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13) +
                    "Помилка збереження відібраних даних", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        ~SelectData()
        {
            try
            {
                if (wordDoc != null) wordDoc.Close(Word.WdSaveOptions.wdPromptToSaveChanges);
                if (wordApp != null) wordApp.Quit(Word.WdSaveOptions.wdPromptToSaveChanges);
            }
            catch { }
        }
    }
}
