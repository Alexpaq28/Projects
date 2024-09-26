import tkinter as tk
import requests
from tkinter import messagebox, ttk
from PIL import Image, ImageTk
import ttkbootstrap

# Fonction pour recevoir meteo du 'API' OpenWeatherMap
def get_weather(city):
    API_key = "8d7808ecb6f8cbdcf9c9494327b307df"
    url = f"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={API_key}"
    res = requests.get(url)

    if res.status_code == 404:
        messagebox.showerror("Error", "City not found")
        return None
    
    # Parse the response JSON to get weather information
    weather = res.json()
    icon_id = weather['weather'] [0] ['icon']
    temperature = weather["main"] ['temp'] - 273.15
    description = weather['weather'] [0] ['description']
    city = weather['name']
    country = weather['sys'] ['country']

    # Get the icon URL and return all the weather information
    icon_url = f"https://openweathermap.org/img/wn/{icon_id}@2x.png"
    return (icon_url, temperature, description, city, country)

# Fonction pour Rechercher meteo d'une 'ville'
def search():
    city = city_entry.get()
    result = get_weather(city)
    if result is None:
        return
    # Si la 'ville' est retrouver, regrouper les infos
    icon_url, temperature, description, city, country = result
    location_label.configure(text=f"{city}, {country}")

    # Recevoir et updater l'icon du label de meteo
    image = Image.open(requests.get(icon_url, stream=True).raw)
    icon = ImageTk.PhotoImage(image)
    icon_label.configure(image=icon)
    icon_label.image = icon

    # Updater les 'labels' pour la temperature et description
    temperature_label.configure(text=f"Température: {temperature:.2f}°C")
    description_label.configure(text=f"Détails: {description}")

# Fonction d'autocompletion / suggestion de resultats
def autocomplete(event):
    entered_text = city_entry.get()
    if len(entered_text) >= 2:
        API_key = "8d7808ecb6f8cbdcf9c9494327b307df"
        url = f"http://api.openweathermap.org/geo/1.0/direct?q={entered_text}&limit=5&appid={API_key}"
        res = requests.get(url)
        if res.status_code == 200:
            cities = res.json()
            suggestions = [f"{city['name']}, {city['country']}" for city in cities]
            autocomplete_listbox.delete(0, tk.END)  # Supprimer les anciennes suggestions
            for suggestion in suggestions:
                autocomplete_listbox.insert(tk.END, suggestion)
            if suggestions:
                autocomplete_listbox.place(x=10, y=90, width=380)
        else:
            messagebox.showerror("Error")

# Fonction pour selectionner resultat du listbox
def select_from_listbox(event):
    selected_index = autocomplete_listbox.curselection()
    if selected_index:
        selected_text = autocomplete_listbox.get(selected_index)
        city_entry.delete(0, tk.END)
        city_entry.insert(0, selected_text)
        autocomplete_listbox.place_forget()


root = ttkbootstrap.Window(themename="morph")
root.title("Weather App")
root.geometry("400x400")
root.config(bg="#87CEEB")

# City name 'entry'
city_entry = ttkbootstrap.Entry(root, font="Helvetica, 18")
city_entry.pack(pady=10)
city_entry.bind("<KeyRelease>", autocomplete)

# Meteo info 'button'
search_button = ttkbootstrap.Button(root, text="Rechercher", command=search, bootstyle="warning")
search_button.pack(pady=10)

# Ville ou Pays 'label'
location_label = tk.Label(root, font="Helvetica, 25", bd=0)
location_label.pack(pady=20)
location_label.config(bg="#87CEEB") 

# Meteo icon 'label'
icon_label = tk.Label(root)
icon_label.pack()
icon_label.config(bg="#87CEEB") 

# Temperature 'label'
temperature_label = tk.Label(root, font="Helvetica, 20", bd=0)
temperature_label.pack()
temperature_label.config(bg="#87CEEB")

# Weather desc 'label'
description_label = tk.Label(root, font="Helvetica, 20", bd=0)
description_label.pack()
description_label.config(bg="#87CEEB") 

# autocomplete 'listbox'
autocomplete_listbox = tk.Listbox(root, font="Helvetica, 18")
autocomplete_listbox.bind("<<ListboxSelect>>", select_from_listbox)
autocomplete_listbox.bind("<FocusOut>", lambda event: autocomplete_listbox.place_forget())

root.mainloop()