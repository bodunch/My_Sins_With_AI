# My Sins Documentation

## Contents
1. [Introduction](#Introduction)
2. [API key connection](#API-key-connection)
3. [How to use](#How-to-use)
4. [Error and exceptions](#Errors-and-exceptions)
5. [API key storage](#API-key-storage)

## Introduction
The app was created for the convenience of remembering and keeping track of your sins. Unfortunately, we are sinful people, so we need to confess regularly, at least when we have committed 10 sins. And Google's AI assistant will help us with this.

## API key connection
When you first launch the program, you will see two windows appear—the main window and the window for entering the API key. Once you have entered the API key correctly, you will not be able to enter it again.

<img width="458" height="259" alt="image" src="https://github.com/user-attachments/assets/25a74ea7-202c-4273-ba91-23eea25a6bca" />

You can generate an API key on the corresponding page of Google AI Studio or by following this [link](https://aistudio.google.com/app/).

You can find out how to create an API key for Gemini in this article at the [link](https://www.merge.dev/blog/gemini-api-key).

## How to use
It's very simple: you just enter your sin in the appropriate field and press **Enter**. Then the request goes to the AI assistant, and it checks whether it is a sin or not.

<img width="400" alt="image" src="https://github.com/user-attachments/assets/b04668e1-3ad6-4828-8114-193e02bca1c0" />

**If you have too many sins, the app will start to change, reminding you that you need to confess and stop sinning:**

<img width="250" alt="image" src="https://github.com/user-attachments/assets/b04668e1-3ad6-4828-8114-193e02bca1c0" />
<img width="250" alt="image" src="https://github.com/user-attachments/assets/2d7fd078-18f0-4eef-ad93-c78145c77b58" /> 
<img width="250" alt="image" src="https://github.com/user-attachments/assets/180f2482-24cb-4c5b-ad5a-945704809be3" />

### But how can we cleanse ourselves of our sins?
To do this, simply click on button **"I Confess My Sins"**, and then, if you have truly confessed, confirm this by clicking **"Yes"**.

On the left, you can see the Settings button, and on the right, the Developers button.

## Errors and exceptions

1. Error: An invalid API key error appears if the API key is entered incorrectly or not entered at all.
<img width="400" alt="image" src="https://github.com/user-attachments/assets/11f6787d-678a-4e28-88ca-2d5a5f113933" />

2. Error: Something happened to your AI assistant.
This error may be related to an invalid API key. Either it was entered incorrectly, or you have reached your API key usage limit. 
<img width="400" alt="image" src="https://github.com/user-attachments/assets/4bd3aa69-ce13-405a-8cf0-36b2162682ae" />

3. Error: The sin has been entered incorrectly. This means that you have disabled the AI assistant. Congratulations!
<img width="400" alt="image" src="https://github.com/user-attachments/assets/2b16860f-0f5f-41e7-91d6-b74a1675a11c" />

4. Error: Your sin limit has been exhausted. Check your limit in Google AI Studio or at this [link](https://aistudio.google.com/app/). Try again in a few seconds.
<img width="400" alt="image" src="https://github.com/user-attachments/assets/0efe2527-fe38-4f1f-b5cf-0b843fd5be7d" />

## API key storage
The API key is stored at:  
> C:\Users\User\AppData\Roaming\MySins  

in the file: **My_API_Key**.

The file is encrypted using the **DPAPI** method and is tied to your computer. Therefore, no matter who you send this file with the key to, they will not be able to decrypt it.







































